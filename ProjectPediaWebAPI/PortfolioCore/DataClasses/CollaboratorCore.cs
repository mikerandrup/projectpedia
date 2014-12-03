using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public class Collaborator_FullItem : IPediaCore_FullItem
    {
        public Collaborator_FullItem(string personId)
        {
            PersonID = personId;
        }

        private string PersonID { get; set; }
        private const string APIPrefix = "/collaborator/";

        public string Name { get; set; }
        public string PrimaryTitle { get; set; }
        public string Biography { get; set; }
        public string Relationship { get; set; }
        public string Contact_LinkedIn { get; set; }
        public string Contact_Twitter { get; set; }
        public string Contact_Website { get; set; }

        public List<Project_ListItem> ProjectList { get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {
            try
            {
                SqlCommand projCmd = DBConnection.CreateCommand();
                projCmd.CommandText = "SELECT * FROM Collaborator WHERE PersonID = @personId";

                projCmd.Parameters.Add(new SqlCeParameter
                {
                    ParameterName = "@personId",
                    Value = PersonID.ToUpper()
                });

                using (var reader = projCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return DatabaseResultCode.notFound;
                    }
                    else
                    {
                        reader.Read();

                        Name = reader["name"].ToString();
                        PrimaryTitle = reader["primary_title"].ToString();
                        Biography = reader["biography"].ToString();
                        Relationship = reader["relationship"].ToString();
                        Contact_LinkedIn = reader["contact_linkedin"].ToString();
                        Contact_Twitter = reader["contact_linkedin"].ToString();
                        Contact_Website = reader["contact_linkedin"].ToString();

                        ProjectList = BuildProjectList(DBConnection);

                        return DatabaseResultCode.okay;
                    }
                }
            }
            catch
            {
                return DatabaseResultCode.miscError;
            }
        }

        private List<Project_ListItem> BuildProjectList(SqlConnection DBConnection)
        {
            var projectList = new List<Project_ListItem>();

            var sqlCommandString =
                "SELECT p.projectid, p.headline, p.byline, p2c.roleheadline, p2c.roledetails " +
                "FROM rel_project2collaborator p2c " +
                "INNER JOIN Project p ON p.projectid = p2c.projectid " +
                "WHERE p2c.personid = @personId";

            SqlCommand cmd = DBConnection.CreateCommand();
            cmd.CommandText = sqlCommandString;

            cmd.Parameters.Add(new SqlCeParameter
            {
                ParameterName = "@personId",
                Value = PersonID
            });

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var CurrentItem = new Project_ListItem(reader["projectid"].ToString());
                    CurrentItem.Headline = reader["headline"].ToString();
                    CurrentItem.Byline = reader["byline"].ToString();

                    projectList.Add(CurrentItem);
                }
            }

            return projectList;
        }
    }

    public class Collaborator_List : IPediaCore_List
    {
        public Collaborator_List()
        {
            ResultList = new List<Collaborator_ListItem>();
        }
        public List<Collaborator_ListItem> ResultList { get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {
            try
            {
                SqlCommand cmd = DBConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Collaborator";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var CurrentItem = new Collaborator_ListItem(reader["personid"].ToString());
                        CurrentItem.Name = reader["name"].ToString();
                        CurrentItem.RoleHeadline = reader["primary_title"].ToString();
                        CurrentItem.RoleDetails = reader["relationship"].ToString();
                        ResultList.Add(CurrentItem);                            
                    }
                }
            }
            catch
            {
                return DatabaseResultCode.miscError;
            }

            if (ResultList.Count > 0)
                return DatabaseResultCode.okay;
            else
                return DatabaseResultCode.notFound;
        }
    }

    public class Collaborator_ListItem : PortfolioCore_BaseClass, IPediaCore_ListItem
    {
        public Collaborator_ListItem(string personId)
        {
            _identity = personId.ToUpper();
            _apiBasePath = "collaborator/";
        }

        public string Name { get; set; }
        public string RoleHeadline { get; set; }
        public string RoleDetails { get; set; }
    }

}