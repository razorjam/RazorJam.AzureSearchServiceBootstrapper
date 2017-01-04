namespace RazorJam.AzureSearchConfiguration.Extensions
{
   using Exceptions;
   using Microsoft.Azure.Search.Models;
   using Options;

   public static class DataSourceConfigurationExtensions
   {
      public static DataSourceType GetDataSourceType( this DataSourceConfiguration dataSourceConfiguration )
      {
         switch( dataSourceConfiguration.Type )
         {
            case "azuresql":
               return DataSourceType.AzureSql;
            case "documentdb":
               return DataSourceType.DocumentDb;
            case "azureblob":
               return DataSourceType.AzureBlob;
            case "azuretable":
               return DataSourceType.AzureTable;
         }

         throw new AzureSearchConfigurationException( $"An invalid data source type was given: {dataSourceConfiguration.Type}" );
      }

      public static bool HasSoftDeletePolicy( this DataSourceConfiguration dataSourceConfiguration )
      {
         return dataSourceConfiguration.DataDeletionDetectionPolicy.Type == "soft";
      }
   }
}