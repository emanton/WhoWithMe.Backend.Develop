using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWithMe.DTO.Meeting
{
    public class CommentMeetingCreateDTO
    {
        public int Estimation { get; set; }
        public string Text { get; set; }
        public long MeetingId { get; set; }
    }

    public class CommentMeetingDeleteDTO
    {
        public long Id { get; set; }
    }
}
