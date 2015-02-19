﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSF.Authorization.Roles;
using WSF.Dependency;
using WSF.Domain.Repositories;
using WSF.MultiTenancy;
using WSF.Runtime.Session;
using Microsoft.AspNet.Identity;

namespace WSF.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// </summary>
    public abstract class WSFUserStore<TTenant, TRole, TUser> :
        IUserPasswordStore<TUser, long>,
        IUserEmailStore<TUser, long>,
        IUserLoginStore<TUser, long>,
        IUserRoleStore<TUser, long>,
        IQueryableUserStore<TUser, long>,
        ITransientDependency
        where TTenant : WSFTenant<TTenant, TUser>
        where TRole : WSFRole<TTenant, TUser>
        where TUser : WSFUser<TTenant, TUser>
    {
        private readonly IRepository<TUser, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<TRole> _roleRepository;
        private readonly IWSFSession _session;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected WSFUserStore(
            IRepository<TUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TRole> roleRepository,
            IWSFSession session)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _session = session;
        }

        public async Task CreateAsync(TUser user)
        {
            await _userRepository.InsertAsync(user);
        }

        public async Task UpdateAsync(TUser user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(TUser user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }

        public async Task<TUser> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user =>
                    user.TenantId == _session.TenantId && //TODO: Should filter automatically when WSF implements it
                    (user.UserName == userName || user.EmailAddress == userName) &&
                    user.IsEmailConfirmed
                );
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.Password = passwordHash;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<string> GetPasswordHashAsync(TUser user)
        {
            return (await _userRepository.GetAsync(user.Id)).Password;
        }

        public async Task<bool> HasPasswordAsync(TUser user)
        {
            return !string.IsNullOrEmpty((await _userRepository.GetAsync(user.Id)).Password);
        }

        public async Task SetEmailAsync(TUser user, string email)
        {
            user.EmailAddress = email;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<string> GetEmailAsync(TUser user)
        {
            return (await _userRepository.GetAsync(user.Id)).EmailAddress;
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return (await _userRepository.GetAsync(user.Id)).IsEmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            if (!user.IsTransient())
            {
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(user => user.EmailAddress == email);
        }

        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }

        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
                );
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return (await _userLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
                );
            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(userLogin.UserId);
        }

        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName && r.TenantId == _session.TenantId);
            await _userRoleRepository.InsertAsync(new UserRole
                                                  {
                                                      UserId = user.Id,
                                                      RoleId = role.Id
                                                  });
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName && r.TenantId == _session.TenantId);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            //TODO: This is not implemented as async.
            var roleNames = _userRoleRepository.Query(userRoles => (from userRole in userRoles
                join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                where userRole.UserId == user.Id
                select role.Name).ToList());

            return Task.FromResult<IList<string>>(roleNames);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            var role = await _roleRepository.SingleAsync(r => r.Name == roleName && r.TenantId == _session.TenantId);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        public IQueryable<TUser> Users
        {
            get { return _userRepository.GetAll(); }
        }

        public void Dispose()
        {
            //No need to dispose since using IOC.
        }
    }
}
