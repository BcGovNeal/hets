/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RoleService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Bulk load of role permissions</remarks>
        /// <param name="items"></param>
        /// <response code="201">Roles created</response>
        public IActionResult RolepermissionsBulkPostAsync(RolePermission[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (RolePermission item in items)
            {
                // adjust the role
                if (item.Role != null)
                {
                    int role_id = item.Role.Id;
                    bool role_exists = _context.Roles.Any(a => a.Id == role_id);
                    if (role_exists)
                    {
                        Role role = _context.Roles.First(a => a.Id == role_id);
                        item.Role = role;
                    }
                }

                // adjust the permission
                if (item.Permission != null)
                {
                    int permission_id = item.Permission.Id;
                    bool permission_exists = _context.Permissions.Any(a => a.Id == permission_id);
                    if (permission_exists)
                    {
                        Permission permission = _context.Permissions.First(a => a.Id == permission_id);
                        item.Permission = permission;
                    }
                }

                var exists = _context.RolePermissions.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RolePermissions.Update(item);
                }
                else
                {
                    _context.RolePermissions.Add(item);
                }
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permissions created</response>
        public IActionResult RolesBulkPostAsync(Role[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (Role item in items)
            {                
                var exists = _context.Roles.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Roles.Update(item);
                }
                else
                {
                    _context.Roles.Add(item);
                }                
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a collection of roles</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesGetAsync()
        {
            var result = _context.Roles.Select(x => x.ToViewModel()).ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdDeletePostAsync(int id)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }
            // remove associated role permission records
            var itemsToRemove = _context.RolePermissions.Where(x => x.Role.Id == role.Id);
            foreach (var item in itemsToRemove)
            {
                _context.RolePermissions.Remove(item);
            }
            _context.Roles.Remove(role);
            _context.SaveChanges();
            return new ObjectResult(role.ToViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdGetAsync(int id)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }
            return new ObjectResult(role.ToViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesIdPermissionsGetAsync(int id)
        {
            // Eager loading of related data
            var role = _context.Roles
                .Where(x => x.Id == id)
                .Include(x => x.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            if (role == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }

            var dbPermissions = role.RolePermissions.Select(x => x.Permission);

            // Create DTO with serializable response
            var result = dbPermissions.Select(x => x.ToViewModel()).ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPermissionsPutAsync(int id, Permission[] items)
        {
            using (var txn = _context.BeginTransaction())
            {
                // Eager loading of related data
                var role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // Not Found
                    return new StatusCodeResult(404);
                }

                var allPermissions = _context.Permissions.ToList();
                var permissionCodes = items.Select(x => x.Code).ToList();
                var existingPermissionCodes = role.RolePermissions.Select(x => x.Permission.Code).ToList();
                var permissionCodesToAdd = permissionCodes.Where(x => !existingPermissionCodes.Contains(x)).ToList();

                // Permissions to add
                foreach (var code in permissionCodesToAdd)
                {
                    var permToAdd = allPermissions.FirstOrDefault(x => x.Code == code);
                    if (permToAdd == null)
                    {
                        // TODO throw new BusinessLayerException(string.Format("Invalid Permission Code {0}", code));
                    }
                    role.AddPermission(permToAdd);
                }

                // Permissions to remove
                List<RolePermission> permissionsToRemove = role.RolePermissions.Where(x => !permissionCodes.Contains(x.Permission.Code)).ToList();
                foreach (RolePermission perm in permissionsToRemove)
                {
                    role.RemovePermission(perm.Permission);
                    _context.RolePermissions.Remove(perm);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                List<RolePermission> dbPermissions = _context.RolePermissions.ToList();

                // Create DTO with serializable response
                var result = dbPermissions.Select(x => x.ToViewModel()).ToList();
                return new ObjectResult(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPermissionsPostAsync(int id, Permission item)
        {
            using (var txn = _context.BeginTransaction())
            {
                // Eager loading of related data
                var role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // Not Found
                    return new StatusCodeResult(404);
                }

                var allPermissions = _context.Permissions.ToList();
                var existingPermissionCodes = role.RolePermissions.Select(x => x.Permission.Code).ToList();
                if (!existingPermissionCodes.Contains(item.Code))
                {
                    var permToAdd = allPermissions.FirstOrDefault(x => x.Code == item.Code);
                    if (permToAdd == null)
                    {
                        // TODO throw new BusinessLayerException(string.Format("Invalid Permission Code {0}", code));
                    }
                    role.AddPermission(permToAdd);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                List<RolePermission> dbPermissions = _context.RolePermissions.ToList();

                // Create DTO with serializable response
                var result = dbPermissions.Select(x => x.ToViewModel()).ToList();
                return new ObjectResult(result);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPermissionsPostAsync(int id, Permission[] items)
        {
            using (var txn = _context.BeginTransaction())
            {
                // Eager loading of related data
                var role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // Not Found
                    return new StatusCodeResult(404);
                }

                var allPermissions = _context.Permissions.ToList();
                var permissionCodes = items.Select(x => x.Code).ToList();
                var existingPermissionCodes = role.RolePermissions.Select(x => x.Permission.Code).ToList();
                var permissionCodesToAdd = permissionCodes.Where(x => !existingPermissionCodes.Contains(x)).ToList();

                // Permissions to add
                foreach (var code in permissionCodesToAdd)
                {
                    var permToAdd = allPermissions.FirstOrDefault(x => x.Code == code);
                    if (permToAdd == null)
                    {
                        // TODO throw new BusinessLayerException(string.Format("Invalid Permission Code {0}", code));
                    }
                    role.AddPermission(permToAdd);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                List<RolePermission> dbPermissions = _context.RolePermissions.ToList();

                // Create DTO with serializable response
                var result = dbPermissions.Select(x => x.ToViewModel()).ToList();
                return new ObjectResult(result);
            }
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPutAsync(int id, RoleViewModel item)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }

            role.Name = item.Name;
            role.Description = item.Description;
            _context.Roles.Update(role);

            // Save changes
            _context.SaveChanges();
            return new ObjectResult(role.ToViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Gets all the users for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesIdUsersGetAsync(int id)
        {            
            List<User> result = new List<User>();

            List<User> users = _context.Users
                    .Include(x => x.UserRoles)
                    .ToList();

            foreach (User user in users)
            {
                bool found = false;

                if (user.UserRoles != null)
                {
                    // ef core does not support lazy loading, so we need to explicitly get data here.
                    foreach (var item in user.UserRoles)
                    {
                        UserRole userRole = _context.UserRoles
                                .Include(x => x.Role)
                                .First(x => x.Id == item.Id);
                        if (userRole.Role.Id == id && userRole.EffectiveDate <= DateTimeOffset.Now && (userRole.ExpiryDate == null || userRole.ExpiryDate > DateTimeOffset.Now))
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found && !result.Contains(user))
                {
                    result.Add(user);
                }
            }

            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the users for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdUsersPutAsync(int id, UserRoleViewModel[] items)
        {
            bool role_exists = _context.Roles.Any(x => x.Id == id);
            if (role_exists)
            {
                Role role = _context.Roles.First(x => x.Id == id);

                foreach (UserRoleViewModel item in items)
                {
                    if (item != null)
                    {
                        // see if there is a matching user
                        bool user_exists = _context.Users.Any(x => x.Id == item.UserId);
                        if (user_exists)
                        {
                            User user = _context.Users.First(x => x.Id == item.UserId);
                            bool found = false;
                            if (user.UserRoles != null)
                            {
                                foreach (UserRole userrole in user.UserRoles)
                                {
                                    if (userrole.Role.Id == item.RoleId)
                                    {
                                        found = true;
                                    }
                                }
                            }

                            if (found == false) // add the user role
                            {
                                UserRole newRole = new UserRole();
                                newRole.Role = role;
                                newRole.EffectiveDate = item.EffectiveDate;
                                newRole.ExpiryDate = null;
                                _context.Add(newRole);

                                if (user.UserRoles == null)
                                {
                                    user.UserRoles = new List<UserRole>();
                                }

                                user.UserRoles.Add(newRole);
                                // update the user.
                                _context.Update(user);
                            }
                        }

                    }
                }
                _context.SaveChanges();
                return new StatusCodeResult(200);
            }
            else
            {
                return new StatusCodeResult(404);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Role created</response>
        public virtual IActionResult RolesPostAsync(RoleViewModel item)
        {
            var role = new Role();
            role.Description = item.Description;
            role.Name = item.Name;

            // Save changes
            _context.Roles.Add(role);
            _context.SaveChanges();
            return new ObjectResult(role.ToViewModel());
        }
    }
}
