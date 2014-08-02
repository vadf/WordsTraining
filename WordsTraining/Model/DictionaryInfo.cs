using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsTraining.Model
{
    public class DictionaryInfo
    {
        public string Name { get; set; }
        public string Language1 { get; private set; }
        public string Language2 { get; private set; }
        public int NumOfCards { get; private set; }
        public WordsDictionary Dictionary { get; private set; }
        public Dictionary<TrainingType, double> TrainingAverage { get; private set; }
        public DataLayer DataLayer { get; private set; }

        public DictionaryInfo(string name, DataLayer data)
        {
            Name = name;
            DataLayer = data;
            Dictionary = DataLayer.Read();
            Language1 = Dictionary.Language1;
            Language2 = Dictionary.Language2;
            NumOfCards = Dictionary.Count;
            TrainingAverage = new Dictionary<TrainingType, double>();
            foreach (var type in WordCard.TrainingTypes)
            {
                TrainingAverage.Add(type, (Dictionary.Average(c => c.Counter1[type]) + Dictionary.Average(c => c.Counter2[type])) / 2.0);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            DictionaryInfo d = obj as DictionaryInfo;
            if ((System.Object)d == null)
            {
                return false;
            }

            return d.Name == this.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}
