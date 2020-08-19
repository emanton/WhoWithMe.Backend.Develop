using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Message : BaseEntity
    {
        public UserChat Chat { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}
