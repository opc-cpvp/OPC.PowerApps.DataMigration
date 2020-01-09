using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC.PowerApps.DataMigration.Configuration
{
    class EntityConfiguration
    {
        public string Name { get; set; }

        public bool DisablePluginsField { get; set; } = false;

        public bool SkipUpdateField { get; set; } = false;

        public bool SkipUpdateFieldSpecified { get; set; } = false;

        public string Filter { get; set; }
    }
}
