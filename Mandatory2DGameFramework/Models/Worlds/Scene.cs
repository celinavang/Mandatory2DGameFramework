using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mandatory2DGameFramework.Model.Worlds
{
    public abstract class Scene
    {
        protected readonly TraceSource _trace;

        public Scene(string filename = "App.config")
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(filename);

            Console.WriteLine(configDoc.DocumentElement?.SelectSingleNode("Name").InnerText);
        }
    }
}
