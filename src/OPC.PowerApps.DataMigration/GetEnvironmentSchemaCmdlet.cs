using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.DataModel.Schema;
using Microsoft.Xrm.Tooling.Dmt.DataMigCommon.Utility;
using Microsoft.Xrm.Tooling.Dmt.MetadataHandler;
using OPC.PowerApps.DataMigration.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;

namespace OPC.PowerApps.DataMigration
{
    [Cmdlet(VerbsCommon.Get, "EnvironmentSchema")]
    public class GetEnvironmentSchemaCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public PSCredential Credentials { get; set; }

        [Parameter(Mandatory = true)]
        public string EnvironmentUrl { get; set; }

        [Parameter(Mandatory = true)]
        public string SchemaOutputPath { get; set; }

        [Parameter]
        public string ConfigurationPath { get; set; }

        [Parameter]
        public string SchemaFileName { get; set; } = "schema.xml";

        private MigrationConfiguration configuration = null;
        private Handler metedataHandler;

        private Dictionary<SolutionInformation, List<Entity>> environmentMetadata = new Dictionary<SolutionInformation, List<Entity>>();

        private static DateMode dateMode = DateMode.absolute;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            // Set the current directory to match PowerShell's current path
            Directory.SetCurrentDirectory(SessionState.Path.CurrentFileSystemLocation.Path);

            if (!string.IsNullOrEmpty(ConfigurationPath))
                configuration = MigrationConfiguration.LoadConfiguration(ConfigurationPath);

            var password = new NetworkCredential(Credentials.UserName, Credentials.Password).Password;
            var connectionString = $"Url={EnvironmentUrl};AuthType=Office365;UserName={Credentials.UserName};Password={password};RequireNewInstance=True;";
            using (var crmServiceClient = new CrmServiceClient(connectionString) { ForceServerMetadataCacheConsistency = true })
            {
                metedataHandler = new Handler(crmServiceClient, null);

                LoadEnvironmentMetadata();
            }
        }

        protected override void ProcessRecord()
        {
            var entities = GetEntities();
            var schemaEntities = ConvertSchemaEntities(entities);
            var outputPath = Path.Combine(SchemaOutputPath, SchemaFileName);
            SerializeSchemaEntities(schemaEntities, outputPath);
        }

        private void LoadEnvironmentMetadata()
        {
            WriteVerbose("Loading environment metadata");
            var solutionMetadata = metedataHandler.GetSolutionMetadata();

            var message = string.Empty;
            var environmentEntities = metedataHandler.GetEntityMetadata(out message);

            foreach (var metadata in solutionMetadata)
            {
                var components = metadata.SolutionComponets;
                var guids = components.Select(e => e.MetadataId).ToList();

                var entities = environmentEntities.Where(e => guids.Contains(e.MetadataId)).ToList();
                foreach (var entity in entities)
                {
                    metedataHandler.GetSetPrimaryFieldDataForEntity(entity);
                    entity.Fields = metedataHandler.GetAttributesForEntity(entity.SchemaName);
                }

                environmentMetadata.Add(metadata, entities);
            }
        }

        private List<Entity> GetEntities()
        {
            WriteVerbose("Getting entities");
            List<Entity> entities = new List<Entity>();

            if (configuration is null)
                foreach (var solutionEntities in environmentMetadata.Values)
                    entities.AddRange(solutionEntities);
            else
                entities = FilterConfigurationEntities();

            return entities.Distinct().ToList();
        }

        private List<Entity> FilterConfigurationEntities()
        {
            WriteVerbose("Filter configuration entities");
            var entities = new List<Entity>();
            foreach (var solutionConfiguration in configuration.Solutions)
            {
                WriteVerbose($"Searching for solution: {solutionConfiguration.Name}");
                // Find the solution that matches our configuration
                var solutionInformation = environmentMetadata.Keys.FirstOrDefault(k => k.UniqueName == solutionConfiguration.Name);

                if (solutionInformation is null)
                {
                    WriteWarning($"Failed to find solution: {solutionConfiguration.Name}");
                    continue;
                }

                WriteVerbose($"Found solution: {solutionConfiguration.Name}");
                WriteVerbose("Filter configuration entities");

                if (!solutionConfiguration.Entities.Any())
                {
                    WriteWarning("No entity filters specified in configuration.");
                    continue;
                }

                var solutionEntities = environmentMetadata[solutionInformation];
                foreach (var entityConfiguration in solutionConfiguration.Entities)
                {
                    WriteVerbose($"Searching for entity: {entityConfiguration}");

                    // Find the entity that matches our solution configuration
                    var entity = solutionEntities.FirstOrDefault(e => e.SchemaName == entityConfiguration.Name);

                    if (entity is null)
                    {
                        WriteWarning($"Failed to find entity: {entityConfiguration}");
                        continue;
                    }

                    WriteVerbose($"Found entity: {entityConfiguration}");
                    entities.Add(entity);
                }
            }

            return entities;
        }

        private List<entitiesEntity> ConvertSchemaEntities(List<Entity> entities)
        {
            WriteVerbose("Converting entities to schema entities");
            return entities.Select(e => new entitiesEntity
            {
                name = e.SchemaName,
                displayname = e.DisplayName,
                etc = e.ETC,
                primaryidfield = e.PrimaryIdName,
                primarynamefield = e.PrimaryFieldName,
                disableplugins = configuration.Solutions.SelectMany(s => s.Entities).First(se => se.Name == e.SchemaName).DisablePluginsField,
                skipupdate = configuration.Solutions.SelectMany(s => s.Entities).First(se => se.Name == e.SchemaName).SkipUpdateField,
                skipupdateSpecified = configuration.Solutions.SelectMany(s => s.Entities).First(se => se.Name == e.SchemaName).SkipUpdateFieldSpecified,
                filter = configuration.Solutions.SelectMany(s => s.Entities).First(se => se.Name == e.SchemaName).Filter,
                fields = e.Fields.Select(f => new entitiesEntityField
                {
                    displayname = f.DisplayName,
                    name = f.SchemaName,
                    type = f.Type,
                    lookupType = f.LookupType,
                    primaryKey = f.PrimaryKey,
                    primaryKeySpecified = f.PrimaryKey,
                    customfield = f.IsCustomField,
                    customfieldSpecified = f.IsCustomField
                }).ToArray()
            }).ToList();
        }

        private void SerializeSchemaEntities(List<entitiesEntity> schemaEntities, string path)
        {
            WriteVerbose("Serializing schema entities");
            string text = Helper.Serialize<entities>(new entities
            {
                entity = schemaEntities.ToArray(),
                dateModeSpecified = (dateMode != DateMode.absolute),
                dateMode = dateMode
            });

            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            text = text.Replace("<?xml version=\"1.0\"?>\r\n", string.Empty);
            text = text.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);
            text = text.Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            text = text.Replace("lookupType=\"\" ", string.Empty);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                byte[] bytes = new UTF8Encoding().GetBytes(text);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
