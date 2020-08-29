using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WhoWithMe.Core.Entities.dictionaries
{
    public class City : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
