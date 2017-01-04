namespace RazorJam.AzureSearchConfiguration.Exceptions
{
   using System;

   public class AzureSearchConfigurationException: Exception
   {
      public AzureSearchConfigurationException( string message ): base( message ) { }
   }
}