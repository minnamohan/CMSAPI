using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMSAPI.Tests
{
    public static class DatabaseSetup
    {
        public static string GetTestSqlServerConnectionString()
        {
            var sqlStr = GetTestConfiguration().GetConnectionString("TestDbConnectionConfigKey");
            return sqlStr;
        }

        public static IConfiguration GetTestConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }

}
