using RestSharp.Authenticators;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace RestSharpTestProject
{
    public class Tests
    {
        RestClient client;
        [SetUp]
        public void Setup()

        {
            var options = new RestClientOptions("https://api.github.com")
            {
              Authenticator = new HttpBasicAuthenticator("TeodoraNalbantova", "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")
            };
            client = new RestClient(options);
        }


        [Test]
        public void Test_GitGetIssuesEndPoint()
        {
            //Arange
          
            var request = new RestRequest
    ("/repos/testnakov/test-nakov-repo/issues", Method.Get);

            //Act
            var responce = client.Get(request);
      
                
            //Assert
            Assert.That (responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void Test_GitIssuesEndpoint_MoreValidation()
        {
            //Arange

            var request = new RestRequest
    ("/repos/testnakov/test-nakov-repo/issues", Method.Get);

            //Act
            var responce = client.Get(request);
            var issuesObjects = JsonConvert.DeserializeObject<List<Issue>>(responce.Content);

            //Assert
            Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issuesObjects.Count, Is.GreaterThan(0));

            Assert.That(issuesObjects[0].number, Is.GreaterThan(0));
            Assert.That(issuesObjects[0].title, Is.Not.Empty);


        }

        private Issue CreateIssue(string title, string body)
        {
            var request = new RestRequest
    ("/repos/testnakov/test-nakov-repo/issues", Method.Post);
            request.AddBody(new { body, title });

            var response = client.Execute(request);
            var issuesObjects = JsonConvert.DeserializeObject<Issue>(response.Content);

            return issuesObjects;

        }
        [Test]
        public void Test_GitPostMethod() 
        {
            //Arrange & Act
            var createdIssue = CreateIssue("IssueTeddy15", "BodyTeddy15");
            //Assert
            Assert.That(createdIssue.title.Equals("IssueTeddy15"));
            Assert.That(createdIssue.body.Equals("BodyTeddy15"));
        }

        [Test]
        public void Test_EditIssue()
        {
            var request = new RestRequest
    ("/repos/testnakov/test-nakov-repo/issues/5002");
            request.AddJsonBody(new
            {
                title = "Changing the name of the issue i created"
            });
            var response = client.Execute(request, Method.Patch);
            var issue = JsonConvert.DeserializeObject<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.body, Is.Not.Empty, "The response content should not be empty.");
            Assert.That(issue.number, Is.GreaterThan(0),"Issue number should be greater than 0.");
            Assert.That(issue.title, Is.EqualTo("Changing the name of the issue i created"));
        }
    }
}