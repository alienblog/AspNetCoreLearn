using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Galaxy.Entities;
using Galaxy.Infrastructure.Core;

namespace Galaxy.Infrastructure.Services.Abstract
{
    public interface IMemberShipService
    {
		MemberShipContext ValidateUser(string username, string password);
		User CreateUser(string username, string email, string password, int[] roles);
		User GetUser(int userId);
		List<Role> GetUserRoles(string username);
	}
}
