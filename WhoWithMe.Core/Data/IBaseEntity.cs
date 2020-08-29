using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Core.Data
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }
}
