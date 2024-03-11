using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;
using System.Globalization;
using OpenQA.Selenium.DevTools.V119.Emulation;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static int lastCreatedIssueNumber;
        private static int lastCreatedCommentId;

        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "TeodoraNalbantova", "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");
            repo = "test-nakov-repo";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            //Arrange

            // Act
            var issues = client.GetAllIssues(repo);
            //Assert
            Assert.That(issues, Has.Count.GreaterThan(0),"There should be more than one issue.");

            foreach (var issue in issues)
            {
                Assert.That(issue.Id,Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0.");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
            }

        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            //Arrange
            int issueNumber = 1;

            // Act
            var issue = client.GetIssueByNumber(repo, issueNumber);
            //Assert
            Assert.IsNotNull(issue, "The response should contain issue data.");
            Assert.That(issue.Id, Is.GreaterThan(0), "The issue ID should be a positive integer.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "The issue number should match the requested number.");

             
            
        }
     
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            //Arrange
            int issueNumber = 10;

            // Act
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);
            //Assert
            Assert.That(labels.Count, Is.GreaterThan(0));
            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be more than 0.");
                Assert.That(label.Name, Is.Not.Null, "Label Name should not be null.");
                Console.WriteLine($"Label: {label.Id} - Name: {label.Name}");
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            //Arrange
            int issueNumber = 10;

            // Act
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);
            //Assert
            Assert.That(comments.Count, Is.GreaterThan(0), "There should be comments on the issue.");
            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be more than 0.");
                Assert.That(comment.Body, Is.Not.Empty, "Comment Body should not be empty.");
                Console.WriteLine($"Comment: {comment.Id} - Body: {comment.Body}");
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            //Arrange
            string title = "New Issue Title";
            string body = "New Body of the New Issue";

            // Act
            var issue = client.CreateIssue(repo, title, body);

            //Assert
            Assert.Multiple(() =>

            {
             Assert.That(issue.Id, Is.GreaterThan(0),"The new issue should have Id.");
             Assert.That(issue.Number, Is.GreaterThan(0));
             Assert.That(issue.Title, Is.Not.Empty);
             Assert.That(issue.Title, Is.EqualTo(title));

            });

            Console.WriteLine(issue.Number);
            lastCreatedCommentId = issue.Number;
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {

            //Arrange
            int issueNumber = lastCreatedCommentId;
            string body = "Body of the new Comment";

            // Act
            var comment = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);

            //Assert
            Assert.That(comment.Body, Is.EqualTo(body));

            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;

        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            //Arrange
            int commentId = lastCreatedCommentId;
           

            // Act
            var comment = client.GetCommentById(repo, commentId);

            //Assert
            Assert.That(comment, Is.Not.Null);
            Assert.That(commentId, Is.EqualTo(comment.Id));
            Assert.That(comment.Body, Is.Not.Empty);


        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            //Arrange
            int commentId = lastCreatedCommentId;
            string newBody = "Edit comment";

            // Act
            var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            //Assert
            Assert.That(comment, Is.Not.Null);
            Assert.That(commentId, Is.EqualTo(comment.Id));
            Assert.That(comment.Body, Is.EqualTo(newBody));

        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            //Arrange
            int commentId = lastCreatedCommentId;

            // Act
            var result = client.DeleteCommentOnGitHubIssue(repo, commentId);

            //Assert
            Assert.That(result, Is.True);
            
        }


    }
}

