namespace RazorJam.AzureSearchConfiguration.Options
{
   // https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Data-Source?redirectedfrom=MSDN
   public class DataSourceConfiguration
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public string Type { get; set; }
      public Credentials Credentials { get; set; }
      public Container Container { get; set; }
      public DataChangeDetectionPolicy DataChangeDetectionPolicy { get; set; }
      public DataDeletionDetectionPolicy DataDeletionDetectionPolicy { get; set; }
   }

   public class Credentials
   {
      public string ConnectionString { get; set; }
   }

   public class Container
   {
      public string Name { get; set; }
      public string Query { get; set; }
   }

   public class DataChangeDetectionPolicy
   {
      public string Type { get; set; }
      public string HighWaterMarkColumnName { get; set; }
   }

   public class DataDeletionDetectionPolicy
   {
      public string Type { get; set; }
      public string SoftDeleteColumnName { get; set; }
      public string SoftDeleteMarkerValue { get; set; }
   }
}
