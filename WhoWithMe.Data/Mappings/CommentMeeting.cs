using WhoWithMe.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace WhoWithMe.Data.Mappings
{
    public class CommentMeetingMapping : EntityTypeConfiguration<CommentMeeting>
    {
        public CommentMeetingMapping()
        {
            ToTable("CommentMeetings");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }

    }
}
