using Galaxy.Entities;
using Galaxy.Infrastructure.Repositories.Abstract;

namespace Galaxy.Infrastructure.Repositories
{
	public class UserRoleRepository : EntityRepository<UserRole>, IUserRoleRepository
	{
		public UserRoleRepository(GalaxyContext context) : base(context)
		{
		}
	}
}
