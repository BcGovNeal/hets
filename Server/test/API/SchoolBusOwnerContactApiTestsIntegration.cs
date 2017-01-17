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
using SchoolBusAPI.Models;
using System.Net;
using Newtonsoft.Json;

namespace SchoolBusAPI.Test
{
	public class SchoolBusOwnerContactApiIntegrationTest 
    { 
		private readonly TestServer _server;
		private readonly HttpClient _client;
			
		/// <summary>
        /// Setup the test
        /// </summary>        
		public SchoolBusOwnerContactApiIntegrationTest()
		{
			_server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());
            _client = _server.CreateClient();
		}
	
		
		[Fact]
		/// <summary>
        /// Integration test for SchoolbusownercontactsBulkPost
        /// </summary>
		public async void TestSchoolbusownercontactsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "api/schoolbusownercontacts/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		
        
		
		[Fact]
		/// <summary>
        /// Integration test for Schoolbusownercontacts
        /// </summary>
		public async void TestSchoolbusOwnerContacts()
		{
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/schoolbusownercontacts");

            // create a new schoolbus.
            SchoolBusOwnerContact schoolBusOwnerContact = new SchoolBusOwnerContact();            
            string jsonString = schoolBusOwnerContact.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            schoolBusOwnerContact = JsonConvert.DeserializeObject<SchoolBusOwnerContact>(jsonString);
            // get the id
            var id = schoolBusOwnerContact.Id; 
            
            // do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/schoolbusownercontacts/" + id);
            request.Content = new StringContent(schoolBusOwnerContact.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/schoolbusownercontacts/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            schoolBusOwnerContact = JsonConvert.DeserializeObject<SchoolBusOwnerContact>(jsonString);
            
            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/schoolbusownercontacts/" + id +"/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/schoolbusownercontacts/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }		
        
		
    }
}
