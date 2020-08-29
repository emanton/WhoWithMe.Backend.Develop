using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWithMe.Core.Entities.Abstract
{
    public abstract class BaseImage : BaseEntity
    {
        public string ImageUrl { get; set; }
    }
}
