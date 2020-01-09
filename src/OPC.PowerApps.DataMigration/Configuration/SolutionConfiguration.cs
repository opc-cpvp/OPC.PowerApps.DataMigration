using System.Collections.Generic;

namespace OPC.PowerApps.DataMigration.Configuration
{
    class SolutionConfiguration
    {
        public string Name { get; set; }
        public List<EntityConfiguration> Entities { get; set; }
    }
}
