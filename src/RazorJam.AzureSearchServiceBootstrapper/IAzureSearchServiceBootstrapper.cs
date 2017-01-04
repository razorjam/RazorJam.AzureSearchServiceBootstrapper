namespace RazorJam.AzureSearchConfiguration
{
   using System.Threading.Tasks;

   public interface IAzureSearchServiceBootstrapper
   {
      Task ConfigureAsync( bool forceRecreate = false );
   }
}