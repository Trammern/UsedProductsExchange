using System;
using RestSharp;
using RestSharp.Authenticators;

namespace UsedProductExchange.Infrastructure.Repositories
{
    public class Mail
    {
        public RestResponse SendSimpleMessage(string email, string subject, string text) {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
                new HttpBasicAuthenticator("api",
                    "0af5b2c05d21be87af49093bc7027ec3-e5da0167-4e5ae533");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox0513afe2e07e4f91aa6be2232cf780f9.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox0513afe2e07e4f91aa6be2232cf780f9.mailgun.org>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);
            request.Method = Method.POST;
            return (RestResponse) client.Execute(request);
        }
    }
}
