using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelConverter
{
    class Program
    {
        private const int MaxNumberPerDictionary = 150;
        static void Main()
        {
            var items = (from line in File.ReadAllLines("hsk_all.csv")
                         let elements = SplitLine(line) 
                         select new DictionaryItem
                             {
                                 Level = int.Parse(elements[0]),
                                 Character = elements[1].Replace(" ", ""),
                                 Pinyin = elements[2].Replace(" ", "").Replace("5", ""),
                                 Translations = elements[3].Split(';').Select(p => p.Trim().Replace("\"", "")).ToList()
                             }).ToList().GroupBy(p => p.Level).ToDictionary(p => p.Key);

            foreach (var key in items.Keys)
            {
                var values = items[key].ToList();
                var numberOfDictionaries = values.Count()/MaxNumberPerDictionary + 1;
                for (int i = 0; i < numberOfDictionaries; i++)
                {
                    string filename;
                    if (numberOfDictionaries > 1)
                    {
                        var appendix = (char)(i + 97);
                        filename = string.Format("hsk{0}{1}.csv", key, appendix);
                    }
                    else
                    {
                        filename = string.Format("hsk{0}.csv", key);
                    }
                    
                    using (var writer = new StreamWriter(filename))
                    {
                        var increment = values.Count/MaxNumberPerDictionary + 1;
                        for (var k = i; k < values.Count; k+=increment)
                        {
                            var dictionaryItem = values.Count() > k ? values[k] : null;
                            if(dictionaryItem == null) continue;
                            writer.Write(dictionaryItem.Character);
                            writer.Write(",");
                            writer.Write(dictionaryItem.Pinyin);
                            writer.Write(",");
                            for (var j = 0; j < dictionaryItem.Translations.Count; j++)
                            {
                                writer.Write(dictionaryItem.Translations[j]);
                                if (j < dictionaryItem.Translations.Count - 1) writer.Write(";");
                            }

                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        private static string[] SplitLine(string line)
        {
            if (!line.Contains("\""))
            {
                return line.Split(',');
            }
            
            var temp = line;
            var firstItem = temp.Substring(0, temp.IndexOf(','));
            temp = temp.Substring(temp.IndexOf(',') + 1);
            var secondItem = temp.Substring(0, temp.IndexOf(','));
            temp = temp.Substring(temp.IndexOf(',') + 1);
            var thirdItem = temp.Substring(0, temp.IndexOf(','));
            temp = temp.Substring(temp.IndexOf(',') + 1);
            var fourthItem = temp.Substring(1, temp.LastIndexOf("\"") - 1);
            return new[] {firstItem, secondItem, thirdItem, fourthItem};
        }
    }

    public class DictionaryItem
    {
        public DictionaryItem()
        {
            Translations = new List<string>();
        }

        public int Level { get; set; }
        public string Character { get; set; }
        public string Pinyin { get; set; }
        public List<string> Translations { get; set; } 
    }
}
