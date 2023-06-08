using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Security
{
    public static class FTPCredentials
    {
        //MMS Test
        //public static string FTPRequestUrl { get { return @"ftp://waws-prod-sg1-007.ftp.azurewebsites.windows.net/site/wwwroot/"; } }
        //public static string FTPUserName { get { return @"g2mmstest\$g2mmstest"; } }
        //public static string FTPPassword { get { return "xzvgvXDp1xAp5biv9hEymxQjjNfGGkzBk3AjWzLlTnEMcvefJenGEnjt6SqT"; ; } }

        //MMSLive
        public static string FTPRequestUrl { get { return @"ftp://waws-prod-sg1-007.ftp.azurewebsites.windows.net/site/wwwroot/"; } }
        public static string FTPUserName { get { return @"cgvakmms\$cgvakmms"; } }
        public static string FTPPassword { get { return "M2BmeKbpuvriDocsf3BaZYAeQj2hwpvGAHABpAyNebDo0i2a7HsbHZN9CS8i"; ; } }


    }
}