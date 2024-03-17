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
	public sealed class ConnectionProperties
	{
		public IConnectionInfo ConnectionInfo { get; private set; }
		public string? ContentPath { get; private set; }

		internal static ObjectCache Cache = MemoryCache.Default;

		internal XElement DriverData => ConnectionInfo.DriverData;

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

		public ServiceClient GetCdsClient()
		{
			return !string.IsNullOrEmpty(ConnectionString)
                ? new ServiceClient(ConnectionString)
                : new ServiceClient(tokenProviderFunction: _ => GetToken(EnvironmentUrl), instanceUrl: new Uri(EnvironmentUrl));
		}

		private static async Task<string> GetToken(string environment)
		{
            DefaultAzureCredential credential = new();

			// TokenProviderFunction is called multiple times, so we need to check if we already have a token in the cache
			if (Cache.Get(environment) is not AccessToken accessToken)
			{
				accessToken = await credential.GetTokenAsync(
                    new TokenRequestContext(new[] { $"{environment}/.default" }));
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
