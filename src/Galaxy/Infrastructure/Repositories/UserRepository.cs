using System.Collections.Generic;
using System.Linq;
using Galaxy.Entities;
using Galaxy.Infrastructure.Repositories.Abstract;

namespace Galaxy.Infrastructure.Repositories
{
    public class UserRepository:EntityRepository<User>,IUserRepository
    {
	    readonly IRoleRepository _roleReposistory;

		public UserRepository(GalaxyContext context, IRoleRepository roleReposistory)
			: base(context)
		{
			_roleReposistory = roleReposistory;
		}

		public User GetSingleByUsername(string username)
		{
			return GetSingle(x => x.Username == username);
		}

		public IEnumerable<Role> GetUserRoles(string username)
		{
			List<Role> roles = null;

			var user = GetSingle(u => u.Username == username, u => u.UserRoles);
			if (user != null)
			{
				roles = user.UserRoles.Select(userRole => _roleReposistory.GetSingle(userRole.RoleId)).ToList();
			}

			return roles;
		}
	}
}
