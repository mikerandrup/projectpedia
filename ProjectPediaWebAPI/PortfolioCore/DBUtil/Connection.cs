using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public static class Connection
    {
        private const string PROJECT_API_DATABASE_NAME = "ProjectPedia";

        private const string AZURE_MANDATED_PREFIX = "SQLCONNSTR_";
        private const string AZURE_DB_STRING_NAME = AZURE_MANDATED_PREFIX + "BEHAVE_DB_STRING";
        private static string AzureConnectionString
        {
            get {
                string connectionStringValue = Environment.GetEnvironmentVariable(AZURE_DB_STRING_NAME);

                if (String.IsNullOrWhiteSpace(connectionStringValue))
                {
                    connectionStringValue = String.Empty;
                }

                // Result of long sad story about Azure & dev machine config.  I plan to reorg my Azure DB setup later.
                connectionStringValue = Regex.Replace(
                    connectionStringValue,
                    BuildDatabaseNameSegment("([A-Za-z]*?)"),
                    BuildDatabaseNameSegment(PROJECT_API_DATABASE_NAME)
                );

                return connectionStringValue;
            }
        }

        private static string BuildDatabaseNameSegment(string valueForDatabaseName)
        {
            return String.Format(";Database={0};", valueForDatabaseName);
        }

        public static SqlConnection Create()
        {
            return new SqlConnection(AzureConnectionString);
        }
    }
}
