using Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.Mappings
{
    public class CommentUserMapping : EntityTypeConfiguration<CommentUser>
    {
        public CommentUserMapping()
        {
            ToTable("CommentUsers");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}