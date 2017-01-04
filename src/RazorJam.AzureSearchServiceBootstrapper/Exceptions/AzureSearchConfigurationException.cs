namespace RazorJam.AzureSearchServiceBootstrapper.Exceptions
{
   using System;

   public class AzureSearchConfigurationException: Exception
   {
      public AzureSearchConfigurationException( string message ): base( message ) { }
   }
}