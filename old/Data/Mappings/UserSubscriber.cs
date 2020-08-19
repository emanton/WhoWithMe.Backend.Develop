using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class UserSubscriberrMapping : EntityTypeConfiguration<UserSubscriber>
    {
        public UserSubscriberrMapping()
        {
            ToTable("UserSubscribers");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}