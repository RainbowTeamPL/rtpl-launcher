using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPonyvilleLauncher.Servers
{
    public class Servers
    {
        public static JSONSchema schema = new JSONSchema();

        public static void GetServers()
        {
            string _JSONSchemaString;

            WebClient webClient = new WebClient();
            _JSONSchemaString = webClient.DownloadString(GlobalVariables.github + "/schema.json");

            schema = JsonConvert.DeserializeObject<JSONSchema>(_JSONSchemaString);
        }

        public class JSONSchema
        {
            public int servercount { get; set; }

            public string server1 { get; set; }
            public string server2 { get; set; }
            public string server3 { get; set; }

            public string password { get; set; }
        }
    }
}