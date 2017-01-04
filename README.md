A tool which bootstraps your azure search service entities (data sources, indexes, and indexers). Intended to be used with configuration as a away to persist changes to your indexes in source control. Works perfectly with dotnetcore, not yet tested on net462.

# Example (dotnetcore)
This example uses dotnetcore and autofac.

## Configuration
First, create your search service in azure. Take note of it's name and admin key, as you'll need these.
Then, simply configure your azure search service in appsettings.json. 

```
{
    "AzureSearchServiceConfiguration": {
        "Name": "required. your search service name",
        "AdminKey": "required. your search service admin key",
        "QueryKey": "optional. the query key for your search service, if you wish to use this configuration elsewhere in your application",
        "DataSource": {
            "Name": "Required. Name of your data source",
            "Type": "Required. Must be one of 'azuresql', 'documentdb', 'azureblob', or 'azuretable'",
            "Credentials": {
                "ConnectionString": "Required. Connection string for your data source"
            },
            "Container": {
                "Name": "Required. Name of the table, collection, or blob container you wish to index",
                "Query": "Optional. Allows you to limit the search to a subset of the data in your container"
            },
            "DataDeletionDetectionPolicy": (optional) {
                "Type": "soft", // currently only 'soft' is available
                "SoftDeleteColumnName": "the column that specifies whether a row was deleted",
                "SoftDeleteMarkerValue": "the value that identifies a row as deleted"
            }
        },
        "Index": {
            "Name": "Required. Name of your index",
            "Fields": [
                ...,
                {
                    "Name": "Required. Name of the field",
                    "Type": "DataType.String | DataType.Int32 | DataType.Int64 | DataType.Double | DataType.Boolean | DataType.DateTimeOffset | DataType.GeographyPoint",
                    "IsKey": true | false (default),
                    "IsRetrievable": true (default) | false,
                    "IsSearchable": true | false (default),
                    "IsFilterable": true | false (default),
                    "IsSortable": true | false (default),
                    "IsFacetable": true | false (default),
                },
                ...
            ]
        },
        "Indexer": {
            "Name": "Required. The name of the indexer",
            "Schedule": {
                "Interval": "Required. An XSD "dayTimeDuration" value (a restricted subset of an ISO 8601 duration value), e.g. 'PT5M' for every 5 minutes. ", // See indexer api docs for more info
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
            // The configuration in appsettings.json is injected into the bootstrapper
            services.AddOptions();
            services.Configure< AzureSearchServiceConfiguration >( options => Configuration.GetSection( "AzureSearchServiceConfiguration" ).Bind( options ) );
            ...
            // Resolve and execute the bootstrapper
            serviceProvider.GetService< IAzureSearchServiceBootstrapper >().ConfigureAsync().Wait();
            ...
        }
    ...
}
```

### Help!
If you're stuck, find a broken thing, or think there needs to be more detail in this readme, get in touch: elliot.chaim@razorjam.co.uk
