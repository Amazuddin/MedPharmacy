using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedPharmacy.Startup))]
namespace MedPharmacy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
