using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Morbs
{
    static class ContentRegister
    {
         public static void RegisterAll()
        {
            Plugin.logger.LogMessage("Registering Content");
            var types = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes().Where(t => t.IsDefined(typeof(ContentAttribute))));//Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(ContentAttribute)));
            foreach ( Type t in types)
            {
                Plugin.logger.LogMessage(t.ToString());
                var methodInfo = t.GetMethod("OnRegister");
                Plugin.logger.LogMessage(methodInfo.ToString());
                if (methodInfo != null)
                {
                    methodInfo.Invoke(null, null);
                }
            }

        }
    }
}
