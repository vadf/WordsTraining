using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsTraining
{
    public enum FilterType { Equals, More, Less }

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

        public static IEnumerable<WordCard> FilterByCounter(IEnumerable<WordCard> dictionary, FilterType type, int filterValue)
        {
            IEnumerable<WordCard> result = dictionary;
            switch (type)
            {
                case FilterType.Equals:
                    result =
                        from c in dictionary
                        where c.Counter1 == filterValue || c.Counter2 == filterValue
                        select c;
                    break;
                case FilterType.More:
                    result =
                        from c in dictionary
                        where c.Counter1 > filterValue || c.Counter2 > filterValue
                        select c;
                    break;
                case FilterType.Less:
                    result =
                        from c in dictionary
                        where c.Counter1 < filterValue || c.Counter2 < filterValue
                        select c;
                    break;
                default:
                    throw new ArgumentException("Unsupported filter type");
            }

            return result;
        }
    }
}
