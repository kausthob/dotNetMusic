using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dotNetMusic.Startup))]
namespace dotNetMusic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
