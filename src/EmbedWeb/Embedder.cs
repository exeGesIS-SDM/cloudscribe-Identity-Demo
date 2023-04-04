using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;

namespace cloudscribe_identity_demo.EmbedWeb
{
    public class Embedder
    {

        private static readonly string _clientId = "SERVICE-PRINCIPAL-CLIENT-ID";
        private static readonly string _clientSecret = "SERVICE-PRINCIPAL-CLIENT-SECRET";
        private static readonly string _tenantId = "SERVICE-PRINCIPAL-TENANT-ID";

        // Power BI Service API Root URL
        const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

        private static string GetAppOnlyAccessToken()
        {
            var tenantAuthority = $"https://login.microsoftonline.com/{_tenantId}";

            var appConfidential = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(tenantAuthority)
                .Build();

            var scopesDefault = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
            var authResult = appConfidential
                .AcquireTokenForClient(scopesDefault)
                .ExecuteAsync()
                .Result;

            return authResult.AccessToken;
        }

        private static PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAppOnlyAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
        }

        public static async Task<EmbedInfo> GetEmbedInfo(Guid workspaceId, Guid reportId)
        {
            var pbiClient = GetPowerBiClient();

            var report = await pbiClient.Reports.GetReportInGroupAsync(workspaceId, reportId);
            var embedUrl = report.EmbedUrl;

            var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "View");

            var tokenResponse = await pbiClient.Reports
                .GenerateTokenInGroupAsync(workspaceId, reportId, generateTokenRequestParameters);

            var embedToken = tokenResponse.Token;

            return new EmbedInfo
            {
                ReportId = report.Id,
                EmbedUrl = embedUrl,
                AccessToken = embedToken
            };

        }
    }

    public class EmbedInfo
    {
        public Guid ReportId { get; set; }
        public string EmbedUrl { get; set; }
        public string AccessToken { get; set; }
    }
}