namespace RazorJam.AzureSearchServiceBootstrapper.Options
{
   // https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Indexer?redirectedfrom=MSDN
   public class IndexerConfiguration
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public Schedule Schedule { get; set; }
      //TODO: other parameters
   }

   public class Schedule
   {
      public string Interval { get; set; }
      public string StartTime { get; set; }
   }
}
