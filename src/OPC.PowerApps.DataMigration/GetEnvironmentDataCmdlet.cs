using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management.Automation;
using System.IO;
using System.Net;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Tooling.Dmt.ExportProcessor.DataInteraction;

namespace OPC.PowerApps.DataMigration
{
    [Cmdlet(VerbsCommon.Get, "EnvironmentData")]
    public class GetEnvironmentDataCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public PSCredential Credentials { get; set; }

        [Parameter(Mandatory = true)]
        public string EnvironmentUrl { get; set; }

        [Parameter(Mandatory = true)]
        public string SchemaPath { get; set; }

        [Parameter]
        public string DataOutputPath { get; set; }

        [Parameter]
        public string DataFileName { get; set; } = "data.zip";

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            // Set the current directory to match PowerShell's current path
            Directory.SetCurrentDirectory(SessionState.Path.CurrentFileSystemLocation.Path);
        }

        protected override void ProcessRecord()
        {
            var password = new NetworkCredential(Credentials.UserName, Credentials.Password).Password;
            var connectionString = $"Url={EnvironmentUrl};AuthType=Office365;UserName={Credentials.UserName};Password={password};RequireNewInstance=True;";
            using (var crmServiceClient = new CrmServiceClient(connectionString) { ForceServerMetadataCacheConsistency = true })
            {
                var dataHandler = new ExportCrmDataHandler(crmServiceClient);

                var dataPath = Path.Combine(DataOutputPath, DataFileName);
                WriteVerbose($"Exporting data file to: {dataPath}");

                dataHandler.ExportData(SchemaPath, dataPath);
            }
        }
    }
}
