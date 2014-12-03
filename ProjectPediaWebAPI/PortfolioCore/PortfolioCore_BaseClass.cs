
namespace ProjectPediaWebAPI.PortfolioCore
{
    public class PortfolioCore_BaseClass
    {
        protected string _identity;

        protected string _apiRoot = "/";
        protected string _apiBasePath;
        public string detailUri
        {
            get { return (_apiRoot + _apiBasePath + _identity).ToLower(); }
            set { }
        }
    }
}