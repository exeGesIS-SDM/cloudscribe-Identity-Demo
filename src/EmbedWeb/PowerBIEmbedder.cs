using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using System;
using System.Threading.Tasks;

namespace cloudscribe_identity_demo.EmbedWeb
{
    public class PowerBIEmbedder : IPowerBIEmbedder
    {
        // reference sources:
        // https://learn.microsoft.com/en-us/power-bi/guidance/powerbi-implementation-planning-usage-scenario-embed-for-your-customers?source=recommendations
        // https://www.codemag.com/Article/1905061/Creating-Embedded-Reports-in-Your-Web-Apps-with-Power-BI-Embedded
        // ** https://github.com/microsoft/PowerBI-Developer-Samples/
        // https://learn.microsoft.com/en-us/power-bi/developer/embedded/embed-tokens?tabs=embed-for-customers
        // ** https://towardsdatascience.com/power-bi-embedded-for-mere-mortals-5478f2f1e58

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;
        private readonly IConfiguration _config;

        public PowerBIEmbedder(
           IConfiguration config
           )
        {
            _config       = config;
            _clientId     = _config.GetValue<string>("AzureOauthSettings:ClientId")     ?? "";
            _clientSecret = _config.GetValue<string>("AzureOauthSettings:ClientSecret") ?? "";
            _tenantId     = _config.GetValue<string>("AzureOauthSettings:TenantId")     ?? "";
        }

        // Power BI Service API Root URL
        const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

        private string GetAppOnlyAccessToken()
        {
            // OAuth token endpoint
            var tenantAuthority = $"https://login.microsoftonline.com/{_tenantId}";


            var appConfidential = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(tenantAuthority)         //.WithB2CAuthority(tenantAuthority)  may need instead?
                .Build();

            var scopesDefault = new string[] { "https://analysis.windows.net/powerbi/api/.default" };
            var authResult = appConfidential
                .AcquireTokenForClient(scopesDefault)
                .ExecuteAsync()
                .Result;

            return authResult.AccessToken;
        }

        private PowerBIClient GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(GetAppOnlyAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
        }

        public async Task<EmbedInfo> GetEmbedInfo(Guid workspaceId, Guid reportId)
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


//////////////// jk - old school way of making OAUTH query ////////////////////////////////////

//var _clientId = "f7ee79fd-86b2-4607-aa2f-25d562e54e23";
//var _clientSecret = "js98Q~vP8aDRdDkyFEEj5PBQIUAOCr4QXlaAcb7X";
//var _tenantId = "400d7813-7145-42a6-9154-2602a956e7f6";

//var tenantAuthority = $"https://login.microsoftonline.com/{_tenantId}"; 

//    using (var client = new HttpClient())
//    {
//    var result = await client.PostAsync(tenantAuthority, new FormUrlEncodedContent(new[] {
//    // new KeyValuePair<string,string>("resource", ResourceUrl),
//    new KeyValuePair<string,string>("client_id", _clientId),
//    new KeyValuePair<string,string>("client_secret", _clientSecret),
//    //new KeyValuePair<string,string>("grant_type", "password"),
//    //new KeyValuePair<string,string>("username", MasterUsername),
//    //new KeyValuePair<string,string>("password", MasterPassword),
//    new KeyValuePair<string,string>("scope", "openid")
//}));

//        var content = await result.Content.ReadAsStringAsync();
//        var content2 = JsonConvert.DeserializeObject(content);
//    }
