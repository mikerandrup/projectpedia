using System;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;

namespace ProjectPediaWebAPI.Controllers
{
    public class AdminController : DatabaseController
    {
        private StringBuilder OutputMessage;

        private int CommandCount { get; set; }
        private int SuccessCount { get; set; }
        private int ErrorCount { get; set; }

        public ActionResult ImportSql()
        {
            OutputMessage = new StringBuilder();

            var MyDirOfSQL = new PortfolioCore.SqlTextLoader(
                AppDomain.CurrentDomain.BaseDirectory + @"sql_import_folder\", "*.sql"
            );

            while (MyDirOfSQL.HasMoreData())
            {
                OutputMessage.Append("<br /><br /><br />---- Processing [" +
                    MyDirOfSQL.GetCurrentFileName() + 
                    "] ---<br /><br />"
                );

                string[] SQLStatementsToRun = MyDirOfSQL.GetNextSetOfQueries();
                for (int i = 0; i < SQLStatementsToRun.Length; i++)
                {
                    if (SQLStatementsToRun[i].Length > 5) // ignore whitespace/linebreak fragments created during parsing
                        RunSQLCommand(SQLStatementsToRun[i]);
                }
            }

            OutputMessage.Insert( 0,
                "Successful SQL Commands: " + SuccessCount + "<br />" +
                "Failed SQL Commands: " + ErrorCount +
                "<br /><br />---------------------------------------------"
            );

            ViewBag.OutputMessage = OutputMessage.ToString(); 

            return View();
        }

        private void RunSQLCommand(string SqlCommand)
        {
            CommandCount++;

            var cmd = new SqlCommand(SqlCommand, _ConnectionToDB);

            try
            {
                OutputMessage.Append("#" + CommandCount + ": [ " + SqlCommand + " ] ");
                cmd.ExecuteNonQuery();
                OutputMessage.Append("<span class='sqlImportSuccess'>--Success!</span><br /><br />");
                SuccessCount++;
            }
            catch (SqlException sqlexception)
            {
                ErrorCount++;
                OutputMessage.AppendFormat("<span class='sqlImportError'> %1 [SqlCeException]", sqlexception.Message);
            }
            catch (Exception exception)
            {
                ErrorCount++;
                OutputMessage.Append(exception.Message + " [Exception]");
            }

        }

    }
}
