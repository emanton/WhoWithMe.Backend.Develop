using Core.Entities.dictionaries;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings.dictionaries
{
    public class MeetingSortTypeMapping : EntityTypeConfiguration<MeetingSortType>
    {
        public MeetingSortTypeMapping()
        {
            ToTable("MeetingSortTypes");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Code).IsRequired().HasMaxLength(256);
        }
    }
}
