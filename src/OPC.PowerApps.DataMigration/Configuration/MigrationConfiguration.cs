using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace OPC.PowerApps.DataMigration.Configuration
{
    class MigrationConfiguration
    {
        public List<SolutionConfiguration> Solutions { get; set; }

        public static MigrationConfiguration LoadConfiguration(string path)
        {
            return JsonConvert.DeserializeObject<MigrationConfiguration>(File.ReadAllText(path));
        }
    }
}
