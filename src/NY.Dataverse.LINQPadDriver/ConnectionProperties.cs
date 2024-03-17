using Azure.Core;
using Azure.Identity;
using LINQPad.Extensibility.DataContext;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace NY.Dataverse.LINQPadDriver
{
    /// <summary>
    /// Wrapper to read/write connection properties. This acts as our ViewModel - we will bind to it in ConnectionDialog.xaml.
    /// </summary>
    [PublicAPI]
    public class ConnectionProperties
    {
        private readonly IConnectionInfo _connectionInfo;

        public IConnectionInfo ConnectionInfo => _connectionInfo;

        public string? ContentPath { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
        protected static ObjectCache Cache = MemoryCache.Default;

        public XElement DriverData => _connectionInfo.DriverData;

        public ConnectionProperties(IConnectionInfo cxInfo)
        {
            _connectionInfo = cxInfo;
        }

        public string ConnectionString
        {
            get
            {
                return AuthenticationType switch
                {
                    "ClientSecret" =>
                        $"AuthType=ClientSecret; Url={EnvironmentUrl}; ClientId={ApplicationId}; ClientSecret={ClientSecret}; RequireNewInstance=true",
                    "Certificate" =>
                        $"AuthType=Certificate; Url={EnvironmentUrl}; ClientId={ApplicationId}; Thumbprint={CertificateThumbprint}; RequireNewInstance=true",
                    "OAuth" =>
                        $"AuthType=OAuth; Url={EnvironmentUrl}; ClientId={ApplicationId}; RedirectUri=http://localhost; LoginPrompt=Auto; TokenCacheStorePath={Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}; RequireNewInstance=true",
                    _ =>
                        string.Empty,
                };
            }
        }

        public string? ApplicationId
        {
            // Default to Microsoft's AppId provided for testing and prototyping
            // as per https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/xrm-tooling/use-connection-strings-xrm-tooling-connect
            get => GetElement("ApplicationId") ?? "51f81489-12ee-4a9e-aaae-a2591f45987d";
            set => DriverData.SetElementValue("ApplicationId", value);
        }

        public string? ClientSecret
        {
            get
            {
                var clientSecret = GetElement("ClientSecret");
                return string.IsNullOrEmpty(clientSecret) ? string.Empty : _connectionInfo.Decrypt(clientSecret);
            }
            set => DriverData.SetElementValue("ClientSecret", _connectionInfo.Encrypt(value));
        }

        public string? CertificateThumbprint
        {
            get
            {
                var thumbprint = GetElement("CertificateThumbprint");
                return string.IsNullOrEmpty(thumbprint)
                    ? string.Empty
                    : _connectionInfo.Decrypt(thumbprint);
            }
            set => DriverData.SetElementValue("CertificateThumbprint", _connectionInfo.Encrypt(value));
        }

        public string EnvironmentUrl
        {
            get => GetElement("EnvironmentUrl") ?? string.Empty;
            set => DriverData.SetElementValue("EnvironmentUrl", value);
        }

        public string AuthenticationType
        {
            get => GetElement("AuthenticationType") ?? "OAuth";
            set
            {
                switch (value)
                {
                    case "ClientSecret":
                        CertificateThumbprint = null;
                        break;
                    case "Certificate":
                        ClientSecret = null;
                        break;
                    case "Azure":
                        ApplicationId = null;
                        CertificateThumbprint = null;
                        ClientSecret = null;
                        break;
                    default:
                        CertificateThumbprint = null;
                        ClientSecret = null;
                        break;
                }
                DriverData.SetElementValue("AuthenticationType", value);
            }
        }

        private string? GetElement(string name) =>
            DriverData.Element(name)?.Value;

        public string ConnectionName
        {
            get => GetElement("ConnectionName") ?? string.Empty;
            set => DriverData.SetElementValue("ConnectionName", value);
        }

        public string UserName
        {
            get => GetElement("UserName") ?? string.Empty;
            set => DriverData.SetElementValue("UserName", value);
        }

        public ServiceClient GetCdsClient()
        {
            return !string.IsNullOrEmpty(ConnectionString)
                ? new ServiceClient(ConnectionString)
                : new ServiceClient(
                    tokenProviderFunction: f => GetToken(EnvironmentUrl),
                    instanceUrl: new Uri(EnvironmentUrl));
        }

        private static async Task<string> GetToken(string environment)
        {
            DefaultAzureCredential credential = new();

            // TokenProviderFunction is called multiple times, so we need to check if we already have a token in the cache
            if (Cache.Get(environment) is not AccessToken accessToken)
            {
                accessToken = await credential.GetTokenAsync(new TokenRequestContext(new[] { $"{environment}/.default" }));
                Cache.Set(
                    environment,
                    accessToken,
                    new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(50)
                    });
            }

            return accessToken.Token;
        }
    }
}
