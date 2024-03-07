using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpDemo;
using System.Text.Json.Serialization;

class Program
{
    static void Main(string[] args)
    {
        //var client = new RestClient("https://api.github.com/");
        
        //var request = new RestRequest("/users/softuni/repos", Method.Get);



        /*URL сегментация
         var request = new RestRequest("/repos/{user}/{repo}/issues/{id}", Method.Get);
        
        request.AddUrlSegment("user", "testnakov");
        request.AddUrlSegment("repo", "test-nakov-repo");
        request.AddUrlSegment("id", "1");
        */
        //-------------------------------------
        //var responce = client.Execute(request);

        //Console.WriteLine(responce.StatusCode);
        //var repoObject = JsonConvert.DeserializeObject<List<Repo>>(responce.Content);

        //foreach (var repo in repoObject)
        //{
        //    Console.WriteLine($"Repo ID: {repo.id}");
        //    Console.WriteLine($"Full Name: {repo.full_name}");
        //    Console.WriteLine($"HTML URL: {repo.html_url}");
        //    Console.WriteLine();
        //}
        // HTTP POST REQUEST//
        var client = new RestClient(new RestClientOptions("https://api.github.com")
        {
            Authenticator = new HttpBasicAuthenticator("TeodoraNalbantova", "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")
        });
        var request = new RestRequest
("/repos/testnakov/test-nakov-repo/issues", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddJsonBody(new { title = "TeddyHasTestingIssue", body = "some body for testing" });

        var response = client.Execute(request);
        Console.WriteLine(response.StatusCode);
    }
}