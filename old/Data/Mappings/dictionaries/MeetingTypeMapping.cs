using Core.Entities.dictionaries;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings.dictionaries
{
    public class MeetingTypeMapping : EntityTypeConfiguration<MeetingType>
    {
        public MeetingTypeMapping()
        {
            ToTable("MeetingTypes");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(256);
        }
    }
}
