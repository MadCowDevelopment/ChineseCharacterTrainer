using System.Data.Entity.ModelConfiguration;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public class AnswerMapping : EntityTypeConfiguration<Answer>
    {
        public AnswerMapping()
        {
            HasKey(p => p.Id);
            HasRequired(p => p.DictionaryEntry).WithMany(p => p.Answers).HasForeignKey(p => p.DictionaryEntryId);
            HasRequired(p => p.QuestionResult).WithMany(p => p.Answers).HasForeignKey(p => p.QuestionResultId);
        }
    }
}