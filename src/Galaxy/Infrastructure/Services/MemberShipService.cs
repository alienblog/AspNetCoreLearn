﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Galaxy.Entities;
using Galaxy.Infrastructure.Core;
using Galaxy.Infrastructure.Repositories.Abstract;
using Galaxy.Infrastructure.Services.Abstract;

namespace Galaxy.Infrastructure.Services
{
	public class MembershipService : IMemberShipService
	{
		#region Variables
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IUserRoleRepository _userRoleRepository;
		private readonly IEncryptionService _encryptionService;
		#endregion
		public MembershipService(IUserRepository userRepository, IRoleRepository roleRepository,
		IUserRoleRepository userRoleRepository, IEncryptionService encryptionService)
		{
			_userRepository = userRepository;
			_roleRepository = roleRepository;
			_userRoleRepository = userRoleRepository;
			_encryptionService = encryptionService;
		}

		#region IMembershipService Implementation

		public MemberShipContext ValidateUser(string username, string password)
		{
			var membershipCtx = new MemberShipContext();

			var user = _userRepository.GetSingleByUsername(username);
			if (user != null && IsUserValid(user, password))
			{
				var userRoles = GetUserRoles(user.Username);
				membershipCtx.User = user;

				var identity = new GenericIdentity(user.Username);
				membershipCtx.Principal = new GenericPrincipal(
					identity,
					userRoles.Select(x => x.Name).ToArray());
			}

			return membershipCtx;
		}
		public User CreateUser(string username, string email, string password, int[] roles)
		{
			var existingUser = _userRepository.GetSingleByUsername(username);

			if (existingUser != null)
			{
				throw new Exception("Username is already in use");
			}

			var passwordSalt = _encryptionService.CreateSalt();

			var user = new User()
			{
				Username = username,
				Salt = passwordSalt,
				Email = email,
				IsLocked = false,
				HashedPassword = _encryptionService.EncryptPassword(password, passwordSalt),
				DateCreated = DateTime.Now
			};

			_userRepository.Add(user);

			_userRepository.Commit();

			if (roles != null || roles.Length > 0)
			{
				foreach (var role in roles)
				{
					AddUserToRole(user, role);
				}
			}

			_userRepository.Commit();

			return user;
		}

		public User GetUser(int userId)
		{
			return _userRepository.GetSingle(userId);
		}

		public List<Role> GetUserRoles(string username)
		{
			var result = new List<Role>();

			var existingUser = _userRepository.GetSingleByUsername(username);

			if (existingUser != null)
			{
				result.AddRange(existingUser.UserRoles.Select(userRole => userRole.Role));
			}

			return result.Distinct().ToList();
		}
		#endregion

		#region Helper methods
		private void AddUserToRole(User user, int roleId)
		{
			var role = _roleRepository.GetSingle(roleId);
			if (role == null)
				throw new Exception("Role doesn't exist.");

			var userRole = new UserRole()
			{
				RoleId = role.Id,
				UserId = user.Id
			};
			_userRoleRepository.Add(userRole);

			_userRepository.Commit();
		}

		private bool IsPasswordValid(User user, string password)
		{
			return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.HashedPassword);
		}

		private bool IsUserValid(User user, string password)
		{
			if (IsPasswordValid(user, password))
			{
				return !user.IsLocked;
			}

			return false;
		}
		#endregion
	}
}
