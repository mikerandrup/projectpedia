using System.Data.SqlClient;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public interface IPediaCore_FullItem
    {
        DatabaseResultCode LoadFromDB(SqlConnection DBConnection);
    }
}