using RestSharp;
using System;
using System.Net;
using TestProjLarge.Entities;

namespace TestProjLarge.Infrastructures
{
    public class Nav
    {
        public static RestClient NavClient (string url, NavApiConfig config)
        {
            if (config.IsNtlmAuthentication)
            {
                var credential = new CredentialCache
                {
                    {
                        new Uri(url),
                        "NTLM",
                        new NetworkCredential(config.NavUserName, config.NavPassword)
                    }
                };
                var client = new RestClient(url)
                {
                    Authenticator = new RestSharp.Authenticators.NtlmAuthenticator(credential)
                };
                client.Timeout = 1200000;
                return client;
            }
            else
            {
                var client = new RestClient(url) {
                    Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(config.NavUserName, config.NavPassword)

                };
                client.Timeout = 1200000;
                return client;
            }
        }
    }
}
