# OPC PowerApps Data Migration

PowerShell cmdlet that wraps Microsoft's [Dynamics 365 Configuration Migration Tool](https://www.nuget.org/packages/Microsoft.CrmSdk.XrmTooling.ConfigurationMigration.Wpf).

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

```
git clone https://github.com/opc-cpvp/OPC.PowerApps.DataMigration.git
```

### Prerequisites

- Microsoft .NET 4.7+
- PowerShell v3+

### Installing

After compiling the class library, simply install the module using the following command:

```
PS> Import-Module OPC.PowerApps.DataMigration.dll
```

## Commands

### Get-EnvironmentSchema

This command is used to extract the schema for a given environment.

```
Get-EnvironmentSchema
    -EnvironmentUrl <String>
    -Credentials <PSCredential>
    -SchemaOutputPath <String>
    [-ConfigurationPath <String>]
    [-SchemaFileName <String>]
    [<CommonParameters>]
```

#### Example
```
PS> $cred = Get-Credential
PS> Get-EnvironmentSchema -EnvironmentUrl "https://<environment-name>.crm3.dynamics.com/" -Credentials $cred -SchemaOutputPath . -ConfigurationPath "configuration.json" -SchemaFileName "environment_schema.xml"
```

## Built With

* [Microsoft .NET](https://dotnet.microsoft.com/) - The framework used

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/opc-cpvp/OPC.PowerApps.BuildTools/tags). 

## Authors

* **OPC-CPVP** - *Initial work* - [OPC-CPVP](https://github.com/opc-cpvp)

See also the list of [contributors](https://github.com/opc-cpvp/OPC.PowerApps.BuildTools/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
