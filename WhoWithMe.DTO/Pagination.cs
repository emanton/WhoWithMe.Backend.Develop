using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.DTO.Model
{
	public abstract class Pagination
	{
		public int Count { get; set; }
		public int Offset { get; set; }
	}
}
