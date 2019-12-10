using System.Collections.Generic;

namespace OPC.PowerApps.DataMigration.Configuration
{
    class SolutionConfiguration
    {
        public string DisplayName { get; set; }
        public List<EntityConfiguration> Entities { get; set; }
    }
}
