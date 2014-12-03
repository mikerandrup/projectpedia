
namespace ProjectPediaWebAPI.PortfolioCore
{
    public class PortfolioCore_BaseClass
    {
        protected string _identity;

        protected string _apiRoot = "/api/";
        protected string _resourcePath;
        public string detailUri
        {
            get { 
                return (_apiRoot + _resourcePath + _identity).ToLower();
            }
        }
    }
}