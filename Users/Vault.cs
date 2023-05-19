using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;

namespace Users;


public class Vault
{
    private readonly string EndPoint;
    private HttpClientHandler httpClientHandler;
    //private IVaultClient vaultClient;
    //private IAuthMethodInfo authMethod;
    




    public Vault()
    {
        EndPoint = "https://localhost:8201/";
        httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback =
        (message, cert, chain, sslPolicyErrors) => { return true; };




    }

    public async Task<string> GetSecret(string path, string key)
    {
        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
        // Initialize settings. You can also set proxies, custom delegates etc.here.
        var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
        {
            Namespace = "",
            MyHttpClientProviderFunc = handler
            => new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(EndPoint)
            }
        };

        IVaultClient vaultClient = new VaultClient(vaultClientSettings);



        // Use client to read a key-value secret.
        Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: path, mountPoint: "secret");

        var secret = kv2Secret.Data.Data[key];

        Console.WriteLine($"MinHemmelighed: {secret}");

        return secret.ToString(); 
    }

    public async Task StoreSalt(string path, string key, byte[] salt)
    {
        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo("00000000-0000-0000-0000-000000000000");
        // Initialize settings. You can also set proxies, custom delegates etc.here.
        var vaultClientSettings = new VaultClientSettings(EndPoint, authMethod)
        {
            Namespace = "",
            MyHttpClientProviderFunc = handler
            => new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(EndPoint)
            }
        };

        IVaultClient vaultClient = new VaultClient(vaultClientSettings);

     var value = new Dictionary<string, object> { { key, salt }};
     var writtenValue = await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync("https://localhost:8201/ui/vault/secrets/secret/show/"+path, value);
    }

}