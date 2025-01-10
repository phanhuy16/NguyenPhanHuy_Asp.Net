using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NguyenPhanHuy_2122110062.Startup))]
namespace NguyenPhanHuy_2122110062
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}