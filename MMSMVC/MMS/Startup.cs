﻿using Microsoft.Owin;
using MMS.Controllers;
using Owin;

[assembly: OwinStartupAttribute(typeof(MMS.Startup))]
namespace MMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);            
        }
    }
}
