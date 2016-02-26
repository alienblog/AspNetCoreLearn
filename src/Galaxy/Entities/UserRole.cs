﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxy.Entities
{
	public class UserRole : IEntity
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public int RoleId { get; set; }

		public virtual Role Role { get; set; }
	}
}
