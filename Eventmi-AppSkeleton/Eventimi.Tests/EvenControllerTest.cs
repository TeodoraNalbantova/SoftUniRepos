using Eventmi.Core.Models.Event;
using Eventmi.Infrastructure.Data.Contexts;
using Eventmi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RestSharp;




namespace Eventmi.Tests
{
    public class Tests
    {
        private RestClient _client;
        private const string _baseUrl = @"https://localhost:7236";


        [SetUp]
        public void Setup()
        {
            _client = new RestClient(_baseUrl);
        }

        [Test]
        public async Task GetAllEvents_ReturnsSuccessStatusCode()
        {
            //Arrange
            var request = new RestRequest("/Event/All", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
        [Test]
        public async Task Add_GetRequest_ReturnAddView()
        {
            //Arrange
            var request = new RestRequest("/Event/Add", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async Task Add_PostRequest_AddEventAndRedirects()
        {
            //Arrange
            var input = new EventFormModel()
            {
                Name = "Teeddy's Event",
                Place = "Teddy's Place",
                Start = new DateTime(2024, 12, 12, 12, 0, 0),
                End = new DateTime(2024, 12, 12, 16, 0, 0)
            };
            var request = new RestRequest("/Event/Add", Method.Post);

            //Act
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt"));

            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.True(CheckEventExist(input.Name), "Event was not added to the database.");

        }



        [Test]
        public async Task Details_GetRequest_ShouldReturnDetailedView()
        {
            //Arrange
            var eventId = 1;
            var request = new RestRequest($"/Event/Details/{eventId}", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));


        }

        [Test]
        public async Task Details_GetRequest_ShouldReturnNotFound_IfNotIdIsGiven()
        {
            //Arrange
            var request = new RestRequest($"/Event/Details/", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request); ;

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task Edit_GetRequest_ShouldReturnEditView()
        {
            //Arrange
            var eventId = 1;
            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }


        [Test]
        public async Task Edit_GetRequest_ShouldReturnNotFoundIfNoIdISGiven()
        {
            //Arrange

            var request = new RestRequest($"/Event/Edit/", Method.Get);

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task Edit_PostRequest_ShouldEditAnEvent ()
            {

            //Arrange
            var eventId = 10;
            var dbEvent = GetEvenById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id,
                Name = dbEvent.Name, 
                Start = dbEvent.Start,
                End = dbEvent.End,
                Place = dbEvent.Place


            };
            string updatedName = "UpdatedEventName";
            input.Name = updatedName;

            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Id", input.Id);
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt"));

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var updatedDbEvent = GetEvenById(eventId);
            Assert.That(updatedDbEvent.Name, Is.EqualTo(input.Name));

        }

        [Test]
        public async Task Edit_WithIdMismatch_ShouldReturnNotFound()
        {
            //Arrange
            var eventId = 10;
            var dbEvent = GetEvenById(eventId);

            var input = new EventFormModel()
            {
                Id = 444,
                Name = dbEvent.Name,
                Start = dbEvent.Start,
                End = dbEvent.End,
                Place = dbEvent.Place


            };

            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("Id", input.Id);
            request.AddParameter("Name", input.Name);
            request.AddParameter("Place", input.Place);
            request.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
            request.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt"));

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task Edit_PostRequest_ShouldReturnBackTheSameViewIfModelErrorsArePresent()
        {

            //Arrange
            var eventId = 1;
            var dbEvent = GetEvenById(eventId);

            var input = new EventFormModel()
            {
                Id = dbEvent.Id, 
                Place = dbEvent.Place,
  
            };

            var request = new RestRequest($"/Event/Edit/{eventId}", Method.Post);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("id", input.Id);
            request.AddParameter("Name", input.Name);
           

            //Act
            var response = await _client.ExecuteAsync(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            

        }

        [Test]
        public async Task Delete_WithValidId_ShouldRedirectToAllItems()
        {
            // arrange
            var input = new EventFormModel()
            {
                Name = "EventForDeleting",
                Start = new DateTime(2024, 12, 12, 12, 0, 0),
                End = new DateTime(2024, 12, 12, 16, 0, 0),
                Place = "Soft Uni",


            };
            var addRequest = new RestRequest("/Event/Add", Method.Post);

            addRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            addRequest.AddParameter("Name", input.Name);
            addRequest.AddParameter("Place", input.Place);
            addRequest.AddParameter("Start", input.Start.ToString("MM/dd/yyyy hh:mm tt"));
            addRequest.AddParameter("End", input.End.ToString("MM/dd/yyyy hh:mm tt"));

           await _client.ExecuteAsync(addRequest);
           
            var eventInDb = GetEventByName(input.Name);
            var eventIdToDelete = eventInDb.Id;

            var deleteRequest = new RestRequest($"/Event/Delete/{eventIdToDelete}", Method.Post);


            // act 
            var response = await _client.ExecuteAsync(deleteRequest);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        private Event GetEventByName(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>()
                .UseSqlServer("Server=TEDDY;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            using var context = new EventmiContext(options);

            return context.Events.FirstOrDefault(n => n.Name == name);
        }
        private bool CheckEventExist(string name)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>()
                .UseSqlServer("Server=TEDDY;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            using var context = new EventmiContext(options);

            return context.Events.Any(e => e.Name == name);
        }

        private Event GetEvenById(int id)
        {
            var options = new DbContextOptionsBuilder<EventmiContext>()
                .UseSqlServer("Server=TEDDY;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            using var context = new EventmiContext(options);
       return context.Events.FirstOrDefault(x => x.Id == id);
            
        }
    }
}