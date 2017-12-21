﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using HETSAPI.Authentication;
using System.IO;
using System.Net.Http;

namespace HETSAPI.Test
{
    public abstract class ApiIntegrationTestBase
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        /// <summary>
        /// Setup the test
        /// </summary>        
        public ApiIntegrationTestBase()
        {
            _server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());          

            //DevAuthenticationOptions devAuthOptions = new DevAuthenticationOptions();
            string testUserName = "TMcTesterson";
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Add("DEV-USER", testUserName);
        }
    }
}
