using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class MessagerMapping : EntityTypeConfiguration<Message>
    {
        public MessagerMapping()
        {
            ToTable("Messages");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}