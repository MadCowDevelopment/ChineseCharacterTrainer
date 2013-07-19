using System.Data.Entity.ModelConfiguration;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public class QuestionResultMapping : EntityTypeConfiguration<QuestionResult>
    {
        public QuestionResultMapping()
        {
            HasKey(p => p.Id);
            HasMany(p => p.Answers).WithRequired(p => p.QuestionResult).HasForeignKey(p => p.QuestionResultId);
        }
    }
}