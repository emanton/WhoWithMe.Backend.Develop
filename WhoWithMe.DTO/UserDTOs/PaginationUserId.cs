using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.DTO;

namespace WhoWithMe.DTO.UserDTOs
{
	public class PaginationUserId : Pagination
	{
		public int UserId { get; set; }
	}
}
