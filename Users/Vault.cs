using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
using Microsoft.Extensions.Configuration; 

namespace Users;


public class Vault
{
    private readonly string EndPoint;
    private HttpClientHandler httpClientHandler;
    //private IVaultClient vaultClient;
    //private IAuthMethodInfo authMethod;
    private readonly IConfiguration _config;




    public Vault()
    {
        EndPoint = _config["Vault_Endpoint"];
        httpClientHandler = new HttpClientHandler();
        httpClientHandler.ServerCertificateCustomValidationCallback =
        (message, cert, chain, sslPolicyErrors) => { return true; };




    }

    public async Task<string> GetSecret(string path, string key)
    {
        // Initialize one of the several auth methods.
        IAuthMethodInfo authMethod = new TokenAuthMethodInfo(_config["Vault_Token"]);
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

        Console.WriteLine($"MySecret: {secret}");

        return secret.ToString(); 
    }

}