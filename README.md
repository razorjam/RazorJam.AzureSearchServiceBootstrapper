A tool which bootstraps your azure search service entities (data sources, indexes, and indexers). Intended to be used with configuration as a away to persist changes to your indexes in source control. Works perfectly with dotnetcore, not yet tested on net462.

# Example (dotnetcore)
This example uses dotnetcore and autofac.

## Configuration
First, create your search service in azure. Take note of it's name and admin key, as you'll need these.
Then, simply configure your azure search service in appsettings.json. 

```
{
    "AzureSearchServiceConfiguration": {
        "Name": "test-search-service",
        "AdminKey": "adminkey",
        "QueryKey": "querykey", // include to use the same config 'SearchService' implementation, if wanted
        "DataSource": {
            "Name": "test-storage-account",
            "Type": "azuretable",
            "Credentials": {
                "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=test-storage-account;AccountKey=keykeykeykey"
            },
            "Container": {
                "Name": "test-table",
                "Query": "RowKey ne 'authentication-details'" // for example
            },
            "DataDeletionDetectionPolicy": {
                "Type": "soft",
                "SoftDeleteColumnName": "IsDeleted",
                "SoftDeleteMarkerValue": "true"
            }
        },
        "Index": {
            "Name": "test-index",
            "Fields": [
                {
                    "Name": "Id",
                    "Type": "DataType.String",
                    "IsKey": true,
                    "IsRetrievable": true
                },
                {
                    "Name": "Name",
                    "Type": "DataType.String",
                    "IsFilterable": true,
                    "IsRetrievable": true
                },
                {
                    "Name": "Gender",
                    "Type": "DataType.String",
                    "IsRetrievable": true,
                    "IsFacetable": true,
                    "IsSearchable": true
                }
                {
                    "Name": "DateCreated",
                    "Type": "DataType.DateTimeOffset",
                    "IsRetrievable": false,
                    "IsSortable": true
                }
            ]
        },
        "Indexer": {
            "Name": "test-indexer",
            "Schedule": {
                "Interval": "PT5M", // an XSD "dayTimeDuration" value (a restricted subset of an ISO 8601 duration value). See indexer api docs
                "StartTime": "now" // if value == "now", will use DateTime.Now, otherwise will use DateTime.Parse( value )
            }
        }
    }
}
```

You may notice that the format here is very, very similar to the request payloads made to the azure search service api. The microsoft documentation for this will be very helpful when using this tool:

Create Data Source API: https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Data-Source?redirectedfrom=MSDN
Create Index API: https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Index?redirectedfrom=MSDN
Create Indexer API: https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Indexer?redirectedfrom=MSDN

## Usage
```
using RazorJam.AzureSearchServiceBootstrapper;
using RazorJam.AzureSearchServiceBootstrapper.Options;

public class Startup
{
    ...
        public IServiceProvider ConfigureServices( IServiceCollection services )
        {
            ...
            serviceProvider.GetService< IAzureSearchServiceBootstrapper >().ConfigureAsync().Wait();
            ...
        }
    ...
}
```

### Help!
If you're stuck, find a broken thing, or think there needs to be more detail in this readme, get in touch: elliot.chaim@razorjam.co.uk