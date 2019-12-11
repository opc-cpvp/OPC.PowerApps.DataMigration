#
# Module manifest for module 'OPC.PowerApps.DataMigration'
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'OPC.PowerApps.DataMigration.psm1'

# Version number of this module.
ModuleVersion = '1.0.0'


# ID used to uniquely identify this module
GUID = 'E8874946-70A1-47B7-9B1C-55B430C286DA'

# Author of this module
Author = 'Office of the Privacy Commissioner Applications Team'


# Company or vendor of this module
CompanyName = 'Office of the Privacy Commissioner'


# Copyright statement for this module
Copyright = '© 2019 Office of the Privacy Commissioner. All rights reserved'


# Description of the functionality provided by this module
Description = 'PowerShell wrapper for Microsoft Dynamics Configuration Migration Tool'


# Minimum version of the Windows PowerShell engine required by this module
#PowerShellVersion = '3.0'

# Name of the Windows PowerShell host required by this module
#PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
#PowerShellHostVersion = ''

# Minimum version of the .NET Framework required by this module
#DotNetFrameworkVersion = '4.0'

# Minimum version of the common language runtime (CLR) required by this module
#CLRVersion = '4.0'

# Processor architecture (None, X86, Amd64) required by this module
#ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
#RequiredModules =

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @(
'Newtonsoft.Json.dll',
'Microsoft.Rest.ClientRuntime.dll'
)

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
#ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
#TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
#FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
NestedModules = @(
	'OPC.PowerApps.DataMigration.dll'
	)

# Functions to export from this module
FunctionsToExport = @()

# Cmdlets to export from this module
CmdletsToExport = @(
  'Get-EnvironmentSchema',
  'Get-EnvironmentData'
)

# Variables to export from this module
VariablesToExport = @(
	)

# Aliases to export from this module
AliasesToExport = @(
	)

# List of all modules packaged with this module.
 ModuleList = @(
	'OPC.PowerApps.DataMigration'
	)

# List of all files packaged with this module
#FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        # Tags = @()

        # A URL to the license for this module.
        # LicenseUri = ''

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/opc-cpvp/OPC.PowerApps.DataMigration'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        # ReleaseNotes = ''

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
#HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
#DefaultCommandPrefix = ''

}