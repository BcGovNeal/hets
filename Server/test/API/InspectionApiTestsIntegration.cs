/*
 * REST API Documentation for Schoolbus
 *
 * API Sample
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using SchoolBusAPI;
using System.Text;
using System.Net;
using SchoolBusAPI.Models;
using Newtonsoft.Json;

namespace SchoolBusAPI.Test
{
	public class InspectionApiIntegrationTest 
    { 
		private readonly TestServer _server;
		private readonly HttpClient _client;
			
		/// <summary>
        /// Setup the test
        /// </summary>        
		public InspectionApiIntegrationTest()
		{
			_server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());
            _client = _server.CreateClient();
		}
	
		
		[Fact]
		/// <summary>
        /// Integration test for InspectionsBulkPost
        /// </summary>
		public async void TestInspectionsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/inspections/bulk");
            request.Content = new StringContent ("[]",Encoding.UTF8,"application/json");
            
			var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();						
		}		
        
		
		[Fact]
		/// <summary>
        /// Integration test for InspectionsGet
        /// </summary>
		public async void TestInspections()
		{
			// first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/inspections");

            // create a new schoolbus.
            Inspection inspection = new Inspection();
            string jsonString = inspection.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            inspection = JsonConvert.DeserializeObject<Inspection>(jsonString);
            // get the id
            var id = inspection.Id;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/inspections/" + id);
            request.Content = new StringContent(inspection.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/inspections/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            inspection = JsonConvert.DeserializeObject<Inspection>(jsonString);
            
            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/inspections/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/inspections/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }		
    }
}
