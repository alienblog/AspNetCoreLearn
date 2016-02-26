using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Galaxy.Infrastructure
{
	public class GalaxyContext : DbContext
	{
		public GalaxyContext()
		{
			
		}

		public GalaxyContext(DbContextOptions options) : base(options)
		{
			
		}
	}
}
