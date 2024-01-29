using Azure.Core;
using Azure.Identity;
using LINQPad.Extensibility.DataContext;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NY.Dataverse.LINQPadDriver
{
	/// <summary>
	/// Wrapper to read/write connection properties. This acts as our ViewModel - we will bind to it in ConnectionDialog.xaml.
	/// </summary>
	sealed class ConnectionProperties
	{
		public IConnectionInfo ConnectionInfo { get; private set; }
		public string? ContentPath { get; private set; }

		static ObjectCache cache = MemoryCache.Default;

		XElement DriverData => ConnectionInfo.DriverData;

		public ConnectionProperties(IConnectionInfo cxInfo) => ConnectionInfo = cxInfo;

		public string ConnectionString
		{
			get
			{
                return AuthenticationType switch
                {
                    "ClientSecret" => $"AuthType=ClientSecret; Url={EnvironmentUrl}; ClientId={ApplicationId}; ClientSecret={ClientSecret}; RequireNewInstance=true",
					"Certificate" => $"AuthType=Certificate; Url={EnvironmentUrl}; ClientId={ApplicationId}; Thumbprint={CertificateThumbprint}; RequireNewInstance=true",
					"OAuth" => $"AuthType=OAuth; Url={EnvironmentUrl}; ClientId={ApplicationId}; RedirectUri=http://localhost; LoginPrompt=Auto; TokenCacheStorePath={Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}; RequireNewInstance=true",
                    _ => "",
                };
            }
		}

		public string? ApplicationId
		{
			get => DriverData.Element("ApplicationId")?.ToString() ?? "51f81489-12ee-4a9e-aaae-a2591f45987d"; //Default to MSFT's AppId provided for testing and prototyping as per https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/xrm-tooling/use-connection-strings-xrm-tooling-connect
			set => DriverData.SetElementValue("ApplicationId", value);
		}

		public string? ClientSecret
		{
			get 
			{ 
				var clientSecret = DriverData.Element("ClientSecret")?.ToString() ?? string.Empty;
				return string.IsNullOrEmpty(clientSecret) ? string.Empty : ConnectionInfo.Decrypt(clientSecret);
			}
			set => DriverData.SetElementValue("ClientSecret", ConnectionInfo.Encrypt(value));
		}
		public string? CertificateThumbprint
		{
			get
			{
				var thumbprint = DriverData.Element("CertificateThumbprint")?.ToString();
				return string.IsNullOrEmpty(thumbprint) ? string.Empty : ConnectionInfo.Decrypt(thumbprint);
			}
			set => DriverData.SetElementValue("CertificateThumbprint", ConnectionInfo.Encrypt(value));
		}
		public string EnvironmentUrl
		{
			get => DriverData.Element("EnvironmentUrl")?.ToString() ?? string.Empty;
			set => DriverData.SetElementValue("EnvironmentUrl", value);
		}

		public string AuthenticationType
		{
			get => DriverData.Element("AuthenticationType")?.ToString() ?? "OAuth";
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
        public string ConnectionName
		{
			get => DriverData.Element("ConnectionName")?.ToString() ?? string.Empty;
			set => DriverData.SetElementValue("ConnectionName", value);
		}
		public string UserName
		{
			get => DriverData.Element("UserName")?.ToString() ?? string.Empty;
			set => DriverData.SetElementValue("UserName", value);
		}

		public ServiceClient GetCdsClient()
		{
			return !string.IsNullOrEmpty(ConnectionString) ? new ServiceClient(ConnectionString) : new ServiceClient(tokenProviderFunction: f => ConnectionProperties.GetToken(EnvironmentUrl), instanceUrl: new Uri(EnvironmentUrl));
		}

		private static async Task<string> GetToken(string environment)
		{
			var credential = new DefaultAzureCredential();
			//TokenProviderFunction is called multiple times, so we need to check if we already have a token in the cache
			var accessToken = cache.Get(environment);
			if (accessToken == null)
			{
				accessToken = (await credential.GetTokenAsync(new TokenRequestContext(new[] { $"{environment}/.default" })));
				cache.Set(environment, accessToken, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(50) });
			}
			return ((AccessToken)accessToken).Token;
		}
	}
}