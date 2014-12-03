using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace ProjectPediaWebAPI.PortfolioCore
{
    public class Skill_FullItem : PortfolioCore_BaseClass, IPediaCore_FullItem
    {
        public Skill_FullItem(string skillId)
        {
            _identity = skillId;
            _apiBasePath = "skill/";
        }

        public string SkillName { get; set; }

        public List<Project_ListItem> ProjectList { get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {

            try
            {
                SqlCommand skillCmd = DBConnection.CreateCommand();
                skillCmd.Parameters.Add(new SqlCeParameter
                {
                    ParameterName = "@skillId",
                    Value = _identity.ToUpper()
                });
                skillCmd.CommandText = "SELECT * FROM Skill WHERE SkillId = @skillId";

                using (var reader = skillCmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return DatabaseResultCode.notFound;
                    }
                    else
                    {
                        reader.Read();

                        SkillName = reader["skillname"].ToString();

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

            SqlCommand cmd = DBConnection.CreateCommand();

            cmd.Parameters.Add(new SqlCeParameter
            {
                ParameterName = "@skillId",
                Value = _identity
            });
            cmd.CommandText =
                "SELECT p.projectid, p.headline, p.byline " +
                "FROM rel_skill2project s2p " +
                "INNER JOIN Project p ON p.projectid = s2p.projectid " +
                "WHERE s2p.skillId = @skillId";

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

    public class Skill_List : IPediaCore_List
    {
        public Skill_List()
        {
            ResultList = new List<Skill_ListItem>();
        }
        public List<Skill_ListItem> ResultList { get; set; }

        public DatabaseResultCode LoadFromDB(SqlConnection DBConnection)
        {
            try
            {
                SqlCommand cmd = DBConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Skill ORDER BY prominence DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var CurrentItem = new Skill_ListItem(reader["skillid"].ToString());
                        CurrentItem.SkillName = reader["skillname"].ToString();
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

    public class Skill_ListItem : PortfolioCore_BaseClass, IPediaCore_ListItem
    {
        public Skill_ListItem(string skillId)
        {
            _identity = skillId;
            _apiBasePath = "skill/";
        }
        private string SkillID { get; set; }
        public string SkillName { get; set; }
    }

}