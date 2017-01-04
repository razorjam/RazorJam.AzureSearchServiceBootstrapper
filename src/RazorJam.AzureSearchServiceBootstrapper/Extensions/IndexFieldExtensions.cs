namespace RazorJam.AzureSearchServiceBootstrapper.Extensions
{
   using Exceptions;
   using Microsoft.Azure.Search.Models;
   using Options;

   public static class IndexFieldExtensions
   {
      public static Field ToField( this IndexField field )
      {
         return new Field( field.Name, field.GetDataType() )
                {
                   IsKey = field.IsKey,
                   IsRetrievable = field.IsRetrievable,
                   IsSearchable = field.IsSearchable,
                   IsFilterable = field.IsFilterable,
                   IsSortable = field.IsSortable,
                   IsFacetable = field.IsFacetable
                };
      }

      public static DataType GetDataType( this IndexField field )
      {
         switch( field.Type )
         {
            case "DataType.Double":
               return DataType.Double;
            case "DataType.Boolean":
               return DataType.Boolean;
            case "DataType.DateTimeOffset":
               return DataType.DateTimeOffset;
            case "DataType.GeographyPoint":
               return DataType.GeographyPoint;
            case "DataType.Int32":
               return DataType.Int32;
            case "DataType.Int64":
               return DataType.Int64;
            case "DataType.String":
               return DataType.String;
         }

         throw new AzureSearchConfigurationException( $"An invalid field type was given: {field.Type}" );
      }
   }
}