using ChineseCharacterTrainer.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChineseCharacterTrainer.ServiceApp
{
    public class SmartEntriesSelector : IEntriesSelector
    {
        public List<DictionaryEntry> SelectEntries(List<DictionaryEntry> entries, QueryObject queryObject)
        {
            if (entries.Count == 0)
            {
                return entries;
            }

            var items = CreateQueryItems(entries);
            CalculateTimeFactor(items);
            return SelectMostRelevantEntries(queryObject, items);
        }

        private static List<QueryItem> CreateQueryItems(IEnumerable<DictionaryEntry> entries)
        {
            var items = new List<QueryItem>();

            foreach (var entry in entries)
            {
                var item = new QueryItem();
                item.DictionaryEntry = entry;
                item.TenMostRecentAnswers = TakeTenMostRecentAnswers(entry);
                item.Ratio = CalculateRatio(item.TenMostRecentAnswers);
                items.Add(item);
            }
            return items;
        }

        private static void CalculateTimeFactor(List<QueryItem> items)
        {
            var allMostRecentAnswers = items.SelectMany(p => p.TenMostRecentAnswers).ToList();
            if (!allMostRecentAnswers.Any())
            {
                foreach (var item in items)
                {
                    item.TimeFactor = 1;
                }
            }
            else
            {
                var oldestAnswerTicks = allMostRecentAnswers.Min(p => p.AnswerTime.Ticks);

                var currentTicks = DateTime.Now.Ticks;
                foreach (var item in items)
                {
                    var oldestAnswer = item.TenMostRecentAnswers.FirstOrDefault();
                    if (oldestAnswer == null)
                    {
                        item.TimeFactor = 1;
                    }
                    else
                    {
                        item.TimeFactor = CalculateTimeFactor(
                            currentTicks,
                            oldestAnswerTicks,
                            oldestAnswer.AnswerTime.Ticks);
                    }
                }
            }
        }

        private static List<DictionaryEntry> SelectMostRelevantEntries(
            QueryObject queryObject, IEnumerable<QueryItem> items)
        {
            return items.OrderBy(p => p.Ratio*p.TimeFactor)
                .Take(queryObject.NumberOfEntries)
                .Select(p => p.DictionaryEntry)
                .ToList();
        }

        private static double CalculateTimeFactor(long currentTicks, long oldestAnswerTicks, long mostRecentAnswerTicks)
        {
            const double factor = 0.5;
            double numerator = currentTicks - mostRecentAnswerTicks;
            double denominator = currentTicks - oldestAnswerTicks;
            return 1 - ((numerator/denominator)*factor);
        }

        private static List<Answer> TakeTenMostRecentAnswers(DictionaryEntry dictionaryEntry)
        {
            return dictionaryEntry.Answers.OrderBy(p => p.AnswerTime).Take(10).ToList();
        }

        private static double CalculateRatio(IReadOnlyList<Answer> answers)
        {
            var numerator = 0.0;
            for (var i = 0; i < answers.Count; i++)
            {
                if (answers[i].IsCorrect)
                {
                    numerator += 1/(Math.Pow(2, i));
                }
            }

            var denominator = Math.Pow(2, answers.Count);

            return numerator/denominator;
        }
        
        private class QueryItem
        {
            public DictionaryEntry DictionaryEntry { get; set; }

            public List<Answer> TenMostRecentAnswers { get; set; }

            public double Ratio { get; set; }

            public double TimeFactor { get; set; }
        }
    }
}