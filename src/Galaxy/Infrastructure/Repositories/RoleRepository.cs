using Galaxy.Entities;
using Galaxy.Infrastructure.Repositories.Abstract;

namespace Galaxy.Infrastructure.Repositories
{
	public class RoleRepository : EntityRepository<Role>, IRoleRepository
	{
		public RoleRepository(GalaxyContext context) : base(context)
		{
		}
	}
}
