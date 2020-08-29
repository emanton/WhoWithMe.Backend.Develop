using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.DTO.Model.Meeting
{
    public class UserMeetingId : CurrentUserTmp
    {
        public int UserId { get; set; }
        public int MeetingId { get; set; }
    }
}
