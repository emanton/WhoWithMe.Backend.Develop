using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class MeetingSubscriberMapping : EntityTypeConfiguration<MeetingSubscriber>
    {
        public MeetingSubscriberMapping()
        {
            ToTable("MeetingSubscribers");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}