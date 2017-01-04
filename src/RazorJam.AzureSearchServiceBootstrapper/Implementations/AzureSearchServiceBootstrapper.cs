namespace RazorJam.AzureSearchServiceBootstrapper.Implementations
{
   using System.Linq;
   using System.Threading.Tasks;
   using System.Xml;
   using Exceptions;
   using Extensions;
   using Microsoft.Azure.Search;
   using Microsoft.Azure.Search.Models;
   using Microsoft.Extensions.Options;
   using Options;

   public class AzureSearchServiceBootstrapper : IAzureSearchServiceBootstrapper
   {
      private readonly DataSourceConfiguration dataSourceConfiguration;
      private readonly IndexConfiguration indexConfiguration;
      private readonly IndexerConfiguration indexerConfiguration;
      private readonly SearchServiceClient searchServiceClient;

      public AzureSearchServiceBootstrapper( IOptions< AzureSearchServiceConfiguration > searchServiceConfiguration )
         : this( searchServiceConfiguration.Value ) { }

      public AzureSearchServiceBootstrapper( AzureSearchServiceConfiguration searchServiceConfiguration )
      {
         this.dataSourceConfiguration = searchServiceConfiguration.DataSource;
         this.indexConfiguration = searchServiceConfiguration.Index;
         this.indexerConfiguration = searchServiceConfiguration.Indexer;

         this.searchServiceClient =
            new SearchServiceClient(searchServiceConfiguration.Name, new SearchCredentials(searchServiceConfiguration.AdminKey));
      }

      public async Task ConfigureAsync( bool forceRecreate = false )
      {
         if( await searchServiceClient.Indexes.ExistsAsync( indexConfiguration.Name ) )
         {
            if( forceRecreate )
            {
               await searchServiceClient.Indexes.DeleteAsync( indexConfiguration.Name );
            }
            else
            {
               return;
            }
         }
         try
         {
            await CreateDataSourceAsync();
            await CreateIndexAsync();
            await CreateNewIndexerAsync();
         }
         catch
         {
            try
            {
               await searchServiceClient.DataSources.DeleteAsync( dataSourceConfiguration.Name );
               await searchServiceClient.Indexes.DeleteAsync( indexConfiguration.Name );
               await searchServiceClient.Indexers.DeleteAsync( indexerConfiguration.Name );
            }
            catch { }
            finally
            {
               throw new AzureSearchConfigurationException( "Configuration was attempted, but failed." );
            }
         }
      }

      private async Task CreateDataSourceAsync()
      {
         if( await searchServiceClient.DataSources.ExistsAsync( dataSourceConfiguration.Name ) )
         {
            await searchServiceClient.DataSources.DeleteAsync(dataSourceConfiguration.Name);
         }

         var dataSource = new DataSource
                          {
                             Name = dataSourceConfiguration.Name,
                             Type = dataSourceConfiguration.GetDataSourceType(),
                             Container = new DataContainer
                                         {
                                            Name = dataSourceConfiguration.Container.Name,
                                            Query = dataSourceConfiguration.Container.Query
                             },
                             Credentials = new DataSourceCredentials { ConnectionString = dataSourceConfiguration.Credentials.ConnectionString }
                          };
         if( dataSourceConfiguration.HasSoftDeletePolicy() )
         {
            dataSource.DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
                                                     {
                                                        SoftDeleteColumnName = dataSourceConfiguration.DataDeletionDetectionPolicy.SoftDeleteColumnName,
                                                        SoftDeleteMarkerValue = dataSourceConfiguration.DataDeletionDetectionPolicy.SoftDeleteMarkerValue
                                                     };
         }
         await searchServiceClient.DataSources.CreateAsync( dataSource );
      }

      private async Task CreateIndexAsync()
      {
         var searchIndex = new Index
                           {
                              Name = indexConfiguration.Name,
                              Fields = indexConfiguration.Fields.Select( x => x.ToField() ).ToList()
                           };
         await searchServiceClient.Indexes.CreateAsync( searchIndex );
      }

      private async Task CreateNewIndexerAsync()
      {
         if (await searchServiceClient.Indexers.ExistsAsync(indexerConfiguration.Name))
         {
            await searchServiceClient.Indexers.DeleteAsync(indexerConfiguration.Name);
         }
         
         var indexer = new Indexer
                       {
                          Name = indexerConfiguration.Name,
                          DataSourceName = dataSourceConfiguration.Name,
                          TargetIndexName = indexConfiguration.Name,
                          Schedule = new IndexingSchedule
                                     {
                                        Interval = XmlConvert.ToTimeSpan( indexerConfiguration.Schedule.Interval ),
                                        StartTime = indexerConfiguration.GetStartTime()
                                     }
                       };
         await searchServiceClient.Indexers.CreateAsync( indexer );
      }
   }
}