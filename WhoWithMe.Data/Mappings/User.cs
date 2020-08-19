using WhoWithMe.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace WhoWithMe.Data.Mappings
{
    public class UserrMapping : EntityTypeConfiguration<User>
    {
        public UserrMapping()
        {
            ToTable("Users");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}