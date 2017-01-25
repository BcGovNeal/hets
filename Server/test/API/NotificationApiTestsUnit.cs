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
	public class NotificationApiUnitTest 
    { 
		
		private readonly NotificationApiController _NotificationApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public NotificationApiUnitTest()
		{
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);

            /*

            Here you will need to mock up the context.

    ItemType fakeItem = new ItemType(...);

    Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

    dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

            */

            NotificationApiService _service = new NotificationApiService(dbAppContext.Object);
			
                    _NotificationApi = new NotificationApiController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsBulkPost
        /// </summary>
		public void TestnotificationsBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsGet
        /// </summary>
		public void TestnotificationsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsIdDelete
        /// </summary>
		public void TestnotificationsIdDelete()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsIdDelete();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsIdGet
        /// </summary>
		public void TestnotificationsIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsIdPut
        /// </summary>
		public void TestnotificationsIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for notificationsPost
        /// </summary>
		public void TestnotificationsPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _NotificationApiController.notificationsPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
