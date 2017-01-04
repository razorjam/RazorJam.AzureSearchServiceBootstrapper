namespace RazorJam.AzureSearchConfiguration.Extensions
{
   using System;
   using Exceptions;
   using Options;

   public static class IndexerConfigurationExtensions
   {
      public static DateTime GetStartTime( this IndexerConfiguration indexerConfiguration )
      {
         if( string.IsNullOrWhiteSpace( indexerConfiguration?.Schedule?.StartTime ) )
         {
            throw new AzureSearchConfigurationException( $"Invalid indexer configuration was found when getting start time." );
         }

         return indexerConfiguration.Schedule.StartTime == "now"
                   ? DateTime.Now
                   : DateTime.Parse( indexerConfiguration.Schedule.StartTime );
      }
   }
}