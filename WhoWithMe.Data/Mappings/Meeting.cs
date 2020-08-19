using WhoWithMe.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace WhoWithMe.Data.Mappings
{
    public class MeetingMapping : EntityTypeConfiguration<Meeting>
    {
        public MeetingMapping()
        {
            ToTable("Meetings");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}