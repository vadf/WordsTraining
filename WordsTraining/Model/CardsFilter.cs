using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsTraining.Model
{
    public enum CounterFilterType { Equals, More, Less }

    public static class CardsFilter
    {
        public static IEnumerable<WordCard> FilterByWord(IEnumerable<WordCard> dictionary, string filterValue)
        {
            var result =
                from c in dictionary
                where c.Word1.ToLower().Contains(filterValue.ToLower()) || c.Word2.ToLower().Contains(filterValue.ToLower())
                select c;
            return result;
        }

        public static IEnumerable<WordCard> FilterByType(IEnumerable<WordCard> dictionary, WordType type)
        {
            var result =
                from c in dictionary
                where c.Type == type
                select c;
            return result;
        }

        public static IEnumerable<WordCard> FilterByCounter(IEnumerable<WordCard> dictionary, CounterFilterType filterType, int filterValue, TrainingType trainingType)
        {
            IEnumerable<WordCard> result = dictionary;
            switch (filterType)
            {
                case CounterFilterType.Equals:
                    result =
                        from c in dictionary
                        where c.Counter1[trainingType] == filterValue || c.Counter2[trainingType] == filterValue
                        select c;
                    break;
                case CounterFilterType.More:
                    result =
                        from c in dictionary
                        where c.Counter1[trainingType] > filterValue || c.Counter2[trainingType] > filterValue
                        select c;
                    break;
                case CounterFilterType.Less:
                    result =
                        from c in dictionary
                        where c.Counter1[trainingType] < filterValue || c.Counter2[trainingType] < filterValue
                        select c;
                    break;
                default:
                    throw new ArgumentException("Unsupported filter type");
            }

            return result;
        }

        public static IEnumerable<WordCard> SortByWord1Asc(IEnumerable<WordCard> dictionary)
        {
            var result =
                from c in dictionary
                orderby c.Word1 
                select c;
            return result;
        }

        public static IEnumerable<WordCard> SortByWord2Asc(IEnumerable<WordCard> dictionary)
        {
            var result =
                from c in dictionary
                orderby c.Word2 
                select c;
            return result;
        }

        public static IEnumerable<WordCard> SortByWord1Desc(IEnumerable<WordCard> dictionary)
        {
            var result =
                from c in dictionary
                orderby c.Word1 descending
                select c;
            return result;
        }

        public static IEnumerable<WordCard> SortByWord2Desc(IEnumerable<WordCard> dictionary)
        {
            var result =
                from c in dictionary
                orderby c.Word2 descending
                select c;
            return result;
        }
    }
}
