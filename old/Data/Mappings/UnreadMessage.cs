using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class UnreadMessagerMapping : EntityTypeConfiguration<UnreadMessage>
    {
        public UnreadMessagerMapping()
        {
            ToTable("UnreadMessages");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}