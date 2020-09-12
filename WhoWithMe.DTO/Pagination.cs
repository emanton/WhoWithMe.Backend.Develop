using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.DTO
{
	public abstract class Pagination// : CurrentUserTmp
	{
		public int Count { get; set; }
		public int Offset { get; set; }
	}
}
