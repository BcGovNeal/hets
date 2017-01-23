/*
 * REST API Documentation for Schoolbus
 *
 * API Sample
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class GroupApiUnitTest 
    { 
		
		private readonly GroupApiController _GroupApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public GroupApiUnitTest()
		{
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);

            /*

            Here you will need to mock up the context.

    ItemType fakeItem = new ItemType(...);

    Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

    dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

            */

            GroupApiService _service = new GroupApiService(dbAppContext.Object);
			
                    _GroupApi = new GroupApiController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for GroupsGet
        /// </summary>
		public void TestGroupsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _GroupApiController.GroupsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
