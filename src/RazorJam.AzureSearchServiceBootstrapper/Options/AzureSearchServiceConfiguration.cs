namespace RazorJam.AzureSearchServiceBootstrapper.Options
{
   public class AzureSearchServiceConfiguration
   {
      public string Name { get; set; }
      public string AdminKey { get; set; }
      public string QueryKey { get; set; }
      public DataSourceConfiguration DataSource { get; set; }
      public IndexConfiguration Index { get; set; }
      public IndexerConfiguration Indexer { get; set; }
   }
}