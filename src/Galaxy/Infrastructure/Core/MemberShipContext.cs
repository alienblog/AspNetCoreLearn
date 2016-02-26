using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Galaxy.Entities;

namespace Galaxy.Infrastructure.Core
{
    public class MemberShipContext
    {
		public IPrincipal Principal { get; set; }
		public User User { get; set; }
		public bool IsValid()
		{
			return Principal != null;
		}
	}
}
