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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI;
using HETSAPI.Models;
using System.Reflection;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model ProjectSearchResultViewModel
    /// </summary>
    
    public class ProjectSearchResultViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for ProjectSearchResultViewModel
        private ProjectSearchResultViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ProjectSearchResultViewModelModelTests()
        {
            instance = new ProjectSearchResultViewModel();
        }

    
        /// <summary>
        /// Test an instance of ProjectSearchResultViewModel
        /// </summary>
        [Fact]
        public void ProjectSearchResultViewModelInstanceTest()
        {
            Assert.IsType<ProjectSearchResultViewModel>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            // TODO unit test for the property 'Id'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'District'
        /// </summary>
        [Fact]
        public void DistrictTest()
        {
            // TODO unit test for the property 'District'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Name'
        /// </summary>
        [Fact]
        public void NameTest()
        {
            // TODO unit test for the property 'Name'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PrimaryContact'
        /// </summary>
        [Fact]
        public void PrimaryContactTest()
        {
            // TODO unit test for the property 'PrimaryContact'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Hires'
        /// </summary>
        [Fact]
        public void HiresTest()
        {
            // TODO unit test for the property 'Hires'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Requests'
        /// </summary>
        [Fact]
        public void RequestsTest()
        {
            // TODO unit test for the property 'Requests'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
			Assert.True(true);
        }

	}
	
}

