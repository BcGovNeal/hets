﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace HETSCommon
{
    public static class VersionInfoExtensions
    {
        public static DatabaseVersionInfo GetDatabaseVersionInfo(this DatabaseFacade database)
        {
            DatabaseVersionInfo info = null;
            DbConnection connection = database.GetDbConnection();

            try
            {
                connection.Open();
                info = new DatabaseVersionInfo()
                {
                    Name = connection.GetType().Name,
                    Version = connection.ServerVersion,
                    Server = connection.DataSource,
                    Database = connection.Database,
                    Migrations = database.GetMigrations(),
                    AppliedMigrations = database.GetAppliedMigrations(),
                    PendingMigrations = database.GetPendingMigrations()
                };
            }
            finally
            {
                connection.Close();
            }

            return info;
        }

        public static ApplicationVersionInfo GetApplicationVersionInfo(this Assembly assembly)
        {
            ApplicationVersionInfo info = new ApplicationVersionInfo()
            {
                Name = assembly.GetName().Name,
                Version = assembly.GetName().Version.ToString(),
                Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright,
                Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description,
                FileVersion = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                FileCreationTime = assembly.GetCreationTime().ToString("MMMM dd, yyyy HH:mm:ss"),
                InformationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                TargetFramework = assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                Title = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title,
                ImageRuntimeVersion = assembly.ImageRuntimeVersion,
                Dependancies = assembly.GetReferencedAssemblies().ToIEnumerableVersionInfo()
            };

            return info;
        }

        private static IEnumerable<VersionInfo> ToIEnumerableVersionInfo(this AssemblyName[] assemblyNames)
        {
            return assemblyNames.Select(d => new VersionInfo() { Name = d.Name, Version = d.Version.ToString() }).ToList();
        }

        private static DateTime GetCreationTime(this Assembly assembly)
        {
            return System.IO.File.GetCreationTime(assembly.Location);
        }
    }
}
