using WhoWithMe.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace WhoWithMe.Data.Mappings
{
    public class UserChatrMapping : EntityTypeConfiguration<UserChat>
    {
        public UserChatrMapping()
        {
            ToTable("UserChats");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}