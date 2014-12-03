using System.Data.SqlClient;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public interface IPediaCore_List
    {
        DatabaseResultCode LoadFromDB(SqlConnection DBConnection);
    }
}