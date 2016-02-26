using System.Collections.Generic;
using Galaxy.Entities;

namespace Galaxy.Infrastructure.Repositories.Abstract
{
	public interface IRoleRepository : IEntityRepository<Role> { }

	public interface IUserRepository : IEntityRepository<User>
	{
		User GetSingleByUsername(string username);
		IEnumerable<Role> GetUserRoles(string username);
	}

	public interface IUserRoleRepository : IEntityRepository<UserRole> { }
}
