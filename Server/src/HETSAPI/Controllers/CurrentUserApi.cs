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
    public partial class CurrentUserApiController : Controller
    {
        private readonly ICurrentUserApiService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public CurrentUserApiController(ICurrentUserApiService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/current")]
        [SwaggerOperation("UsersCurrentGet")]
        [SwaggerResponse(200, type: typeof(CurrentUserViewModel))]
        public virtual IActionResult UsersCurrentGet()
        {
            return this._service.UsersCurrentGetAsync();
        }
    }
}
