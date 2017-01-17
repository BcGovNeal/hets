/*
 * REST API Documentation for the MOTI School Bus Application
 *
 * The School Bus application tracks that inspections are performed in a timely fashion. For each school bus the application tracks information about the bus (including data from ICBC, NSC, etc.), it's past and next inspection dates and results, contacts, and the inspector responsible for next inspecting the bus.
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
using Newtonsoft.Json;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserApiController : Controller
    {
        private readonly IUserApiService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public UserApiController(IUserApiService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of user groups</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/usergroups/bulk")]
        [SwaggerOperation("UsergroupsBulkPost")]
        public virtual IActionResult UsergroupsBulkPost([FromBody]GroupMembership[] items)
        {
            return this._service.UsergroupsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of user roles</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/userroles/bulk")]
        [SwaggerOperation("UserrolesBulkPost")]
        public virtual IActionResult UserrolesBulkPost([FromBody]UserRole[] items)
        {
            return this._service.UserrolesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of users</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/users/bulk")]
        [SwaggerOperation("UsersBulkPost")]
        public virtual IActionResult UsersBulkPost([FromBody]User[] items)
        {
            return this._service.UsersBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all users</remarks>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users")]
        [SwaggerOperation("UsersGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult UsersGet()
        {
            return this._service.UsersGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Deletes a user</remarks>
        /// <param name="id">id of User to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [Route("/api/users/{id}/delete")]
        [SwaggerOperation("UsersIdDeletePost")]
        public virtual IActionResult UsersIdDeletePost([FromRoute]int id)
        {
            return this._service.UsersIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given context type</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/favourites")]
        [SwaggerOperation("UsersIdFavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavouriteViewModel>))]
        public virtual IActionResult UsersIdFavouritesGet([FromRoute]int id)
        {
            return this._service.UsersIdFavouritesGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns data for a particular user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdGet")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersIdGet([FromRoute]int id)
        {
            return this._service.UsersIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all groups that a user is a member of</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsGet")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsGet([FromRoute]int id)
        {
            return this._service.UsersIdGroupsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add to the active set of groups for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsPost")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsPost([FromRoute]int id, [FromBody]GroupMembership[] items)
        {
            return this._service.UsersIdGroupsPostAsync(id, items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the active set of groups for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsPut")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsPut([FromRoute]int id, [FromBody]GroupMembership[] items)
        {
            return this._service.UsersIdGroupsPutAsync(id, items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a user&#39;s notifications</remarks>
        /// <param name="id">id of User to fetch notifications for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/{id}/notifications")]
        [SwaggerOperation("UsersIdNotificationsGet")]
        [SwaggerResponse(200, type: typeof(List<NotificationViewModel>))]
        public virtual IActionResult UsersIdNotificationsGet([FromRoute]int id)
        {
            return this._service.UsersIdNotificationsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/permissions")]
        [SwaggerOperation("UsersIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        public virtual IActionResult UsersIdPermissionsGet([FromRoute]int id)
        {
            return this._service.UsersIdPermissionsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdPut")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersIdPut([FromRoute]int id, [FromBody]UserViewModel item)
        {
            return this._service.UsersIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesGet")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult UsersIdRolesGet([FromRoute]int id)
        {
            return this._service.UsersIdRolesGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="201">Role created for user</response>
        [HttpPost]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPost")]
        [SwaggerResponse(200, type: typeof(UserRoleViewModel))]
        public virtual IActionResult UsersIdRolesPost([FromRoute]int id, [FromBody]UserRoleViewModel item)
        {
            return this._service.UsersIdRolesPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPut")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult UsersIdRolesPut([FromRoute]int id, [FromBody]UserRoleViewModel[] items)
        {
            return this._service.UsersIdRolesPutAsync(id, items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Create new user</remarks>
        /// <param name="item"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/users")]
        [SwaggerOperation("UsersPost")]
        [SwaggerResponse(200, type: typeof(User))]
        public virtual IActionResult UsersPost([FromBody]User item)
        {
            return this._service.UsersPostAsync(item);
        }
    }
}
