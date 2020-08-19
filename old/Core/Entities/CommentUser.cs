using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CommentUser : BaseEntity
    {
        public int Estimation { get; set; }
        public User Creator { get; set; }
        public User User { get; set; }
    }
}
