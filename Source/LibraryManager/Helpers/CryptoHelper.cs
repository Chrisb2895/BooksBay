using Microsoft.AspNetCore.DataProtection;

namespace LibraryManager.Helpers
{
    public class CryptoHelper
    {
        IDataProtector _protector;

        // the 'provider' parameter is provided by DI
        public CryptoHelper(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("LibraryManager");
        }

        public void RunSample()
        {
            /*Console.Write("Enter input: ");
            string input = Console.ReadLine();

            // protect the payload
            string protectedPayload = _protector.Protect(input);
            Console.WriteLine($"Protect returned: {protectedPayload}");

            // unprotect the payload
            string unprotectedPayload = _protector.Unprotect(protectedPayload);
            Console.WriteLine($"Unprotect returned: {unprotectedPayload}");*/
        }


        public  string GetCrypted(string fromS)
        {
            return _protector.Protect(fromS);
        }

        public  string GetUnCrypted(string fromS)
        {
            return _protector.Unprotect(fromS);
        }
    }
}
