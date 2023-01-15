using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Graph;
using System.Security.Cryptography.X509Certificates;

namespace BlazorBffAzureB2C.Server.Services;

public class GraphApplicationClientService
{
    private readonly IConfiguration _configuration;
    private GraphServiceClient? _graphServiceClient;

    public GraphApplicationClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Using client secret from a singleton instance
    /// </summary>
    /// <returns></returns>
    public GraphServiceClient GetGraphClientWithClientSecretCredential()
    {
        if (_graphServiceClient != null)
            return _graphServiceClient;

        string[]? scopes = _configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ');
        var tenantId = _configuration.GetValue<string>("GraphApi:TenantId");

        // Values from app registration
        var clientId = _configuration.GetValue<string>("GraphApi:ClientId");
        var clientSecret = _configuration.GetValue<string>("GraphApi:ClientSecret");

        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        return _graphServiceClient;
    }

    /// <summary>
    /// Using Graph SDK client with a certificate
    /// https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-net-client-assertions
    /// </summary>
    public async Task<GraphServiceClient> GetGraphClientWithClientCertificateCredentialAsync()
    {
        if (_graphServiceClient != null)
            return _graphServiceClient;

        string[]? scopes = _configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ');
        var tenantId = _configuration.GetValue<string>("GraphApi:TenantId");

        // Values from app registration
        var clientId = _configuration.GetValue<string>("GraphApi:ClientId");

        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        var certififacte = await GetCertificateAsync();
        var clientCertificateCredential = new ClientCertificateCredential(
            tenantId, clientId, certififacte, options);

        // var clientCertificatePath = _configuration.GetValue<string>("AzureAd:CertificateName");
        // https://learn.microsoft.com/en-us/dotnet/api/azure.identity.clientcertificatecredential?view=azure-dotnet
        // var clientCertificateCredential = new ClientCertificateCredential(
        //    tenantId, clientId, clientCertificatePath, options);

         _graphServiceClient = new GraphServiceClient(clientCertificateCredential, scopes);
        return _graphServiceClient;
    }

    private async Task<X509Certificate2> GetCertificateAsync()
    {
        var identifier = _configuration["GraphApi:ClientCertificates:0:KeyVaultCertificateName"];

        if (identifier == null)
            throw new ArgumentNullException(nameof(identifier));

        var vaultBaseUrl = _configuration["GraphApi:ClientCertificates:0:KeyVaultUrl"];
        if(vaultBaseUrl == null)
            throw new ArgumentNullException(nameof(vaultBaseUrl));

        var secretClient = new SecretClient(vaultUri: new Uri(vaultBaseUrl), credential: new DefaultAzureCredential());

        // Create a new secret using the secret client.
        var secretName = identifier;
        //var secretVersion = "";
        KeyVaultSecret secret = await secretClient.GetSecretAsync(secretName);

        var privateKeyBytes = Convert.FromBase64String(secret.Value);

        var certificateWithPrivateKey = new X509Certificate2(privateKeyBytes,
            string.Empty, X509KeyStorageFlags.MachineKeySet);

        return certificateWithPrivateKey;
    }
}
