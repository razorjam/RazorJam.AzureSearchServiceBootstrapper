namespace RazorJam.AzureSearchServiceBootstrapper.Options
{
   using System.Collections.Generic;

   // https://docs.microsoft.com/en-us/rest/api/searchservice/Create-Index?redirectedfrom=MSDN
   public class IndexConfiguration
   {
      public string Name { get; set; }
      public List< IndexField > Fields { get; set; }
      //TODO: suggesters, analyzers
   }

   public class IndexField
   {
      public string Name { get; set; }
      public string Type { get; set; }
      public bool IsKey { get; set; }
      public bool IsRetrievable { get; set; } = true;
      public bool IsSearchable { get; set; }
      public bool IsFilterable { get; set; }
      public bool IsSortable { get; set; }
      public bool IsFacetable { get; set; }
   }
}
