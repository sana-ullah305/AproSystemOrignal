using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprosysAccounting
{
    public static class Config
    {
        public static bool AllowEditDate { get
            {

                return string.Compare(System.Configuration.ConfigurationManager.AppSettings["AllowEditDate"], "true", true) == 0;
            } }
    }
}