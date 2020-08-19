using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Core.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public long Id { get; set; }
    }
}
