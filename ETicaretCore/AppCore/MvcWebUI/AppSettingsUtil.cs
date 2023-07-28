using AppCore.MvcWebUI.Bases;
using Microsoft.Extensions.Configuration;

namespace AppCore.MvcWebUI
{
    public class AppSettingsUtil : AppSettingsUtilBase
    {
        public AppSettingsUtil(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
