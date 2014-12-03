using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectPediaWebAPI.DBUtil
{
    public enum DbResult
    {
        Unknown = -1,
        Okay = 1,
        NotFound = 2,
        Error = 3
    }
}