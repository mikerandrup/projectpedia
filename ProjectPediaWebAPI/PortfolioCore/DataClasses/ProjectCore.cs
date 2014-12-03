using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public class Project_FullItem : PortfolioCore_BaseClass, IPediaCore_FullItem
    {
        public Project_FullItem(string projectId)
        {
            _identity = projectId;
            _resourcePath = "project/";
        }

        public string Headline { get; set; }
        public string Byline { get; set; }
        public string Details { get; set; }

        public List<Collaborator_ListItem> CollaboratorList{ get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {
            try
            {
                SqlCommand projCmd = DBConnection.CreateCommand();
                projCmd.CommandText = "SELECT * FROM Project WHERE ProjectId = @identity";

                projCmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@identity",
                    Value = _identity.ToUpper()
                });

                using (var projReader = projCmd.ExecuteReader())
                {
                    if (projReader.HasRows && projReader.Read())
                    {
                        Headline = projReader["headline"].ToString();
                        Byline = projReader["byline"].ToString();
                        Details = projReader["details"].ToString();
                    }
                    else
                    {
                        return DatabaseResultCode.notFound;
                    }
                }

                CollaboratorList = BuildCollaboratorList(DBConnection);

                return DatabaseResultCode.okay;
            }
            catch (Exception exc)
            {
                return DatabaseResultCode.miscError;
            }
        }

        private List<Collaborator_ListItem> BuildCollaboratorList(SqlConnection DBConnection)
        {
            var collabList = new List<Collaborator_ListItem>();

            var sqlCommandString =
                "SELECT c.personid, c.name, c.primary_title, p2c.roleheadline, p2c.roledetails " +
                "FROM rel_project2collaborator p2c " +
                "INNER JOIN Collaborator c ON c.personid = p2c.personid " +
                "WHERE p2c.projectid = @identity";

            SqlCommand cmd = DBConnection.CreateCommand();
            cmd.CommandText = sqlCommandString;

            cmd.Parameters.Add(new SqlParameter {
                ParameterName = "@identity",
                Value = _identity
            });

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var CurrentItem = new Collaborator_ListItem(reader["personid"].ToString());
                    CurrentItem.RoleDetails = reader["roledetails"].ToString();
                    CurrentItem.Name = reader["name"].ToString();

                    CurrentItem.RoleHeadline = reader["primary_title"].ToString();
                    string overrideHeadline = reader["roleheadline"].ToString();
                    if (!String.IsNullOrEmpty(overrideHeadline))
                        CurrentItem.RoleHeadline = overrideHeadline;

                    collabList.Add( CurrentItem );
                }
            }

            return collabList;
        }
    }

    public class Project_List : IPediaCore_List
    {
        public Project_List()
        {
            ResultList = new List<Project_ListItem>();
        }
        public List<Project_ListItem> ResultList { get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {
            try
            {
                SqlCommand cmd = DBConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Project";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var currentItem = new Project_ListItem(reader["projectid"].ToString());
                        currentItem.Headline = reader["headline"].ToString();
                        currentItem.Byline = reader["byline"].ToString();
                        ResultList.Add(currentItem);
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

    public class Project_ListItem : PortfolioCore_BaseClass, IPediaCore_ListItem
    {
        public Project_ListItem(string projectId)
        {
            _identity = projectId;
            base._resourcePath = "project/";
        }

        public Project_ListItem(string projectId, string headline, string byline)
        {
            _identity = projectId;
            Headline = headline;
            Byline = byline;
        }

        public string Headline { get; set; }
        public string Byline { get; set; }
    }
}