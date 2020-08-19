using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class LogMapping : EntityTypeConfiguration<Log>
    {
        public LogMapping()
        {
            ToTable("Logs");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}