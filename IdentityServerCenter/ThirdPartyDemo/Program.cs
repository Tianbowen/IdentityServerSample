using System;
using static System.Console;
using IdentityModel;
using IdentityModel.Client;
using System.Net.Http;

namespace ThirdPartyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var disco= client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;

            if (disco.IsError)
            {
                WriteLine(disco.Error);
            }

            WriteLine(disco.TokenEndpoint);
          var tokenResponse=  client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            { 
                Address=disco.TokenEndpoint,
                ClientId="client",
                ClientSecret="secret",
                Scope="api"

            }).Result;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }
            WriteLine(tokenResponse.AccessToken);

            var apiReponse = new HttpClient();
            apiReponse.SetBearerToken(tokenResponse.AccessToken);
            var result= apiReponse.GetAsync("http://localhost:5001/api/values").Result;
            WriteLine(result.IsSuccessStatusCode);
            if (result.IsSuccessStatusCode)
            {
                var ss= result.Content.ReadAsStringAsync().Result;
                WriteLine(ss);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
