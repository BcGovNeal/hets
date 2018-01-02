﻿using System.Linq;

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Role
    {
        /// <summary>
        /// Adds a permission to this role instance.
        /// </summary>
        /// <param name="permission">The permission to add.</param>
        public virtual void AddPermission(Permission permission)
        {
            var rolePermission = new RolePermission
            {
                Permission = permission,
                Role = this
            };
            RolePermissions.Add(rolePermission);
        }

        /// <summary>
        /// Removes a permission from this role instance.
        /// </summary>
        /// <param name="permission">The permission to remove.</param>
        public virtual void RemovePermission(Permission permission)
        {
            var permissionToRemove = RolePermissions.FirstOrDefault(x => x.Permission.Code == permission.Code);
            if (permissionToRemove != null)
            {
                RolePermissions.Remove(permissionToRemove);
            }
        }
    }
}
