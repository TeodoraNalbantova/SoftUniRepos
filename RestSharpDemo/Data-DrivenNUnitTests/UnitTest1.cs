using RestSharp;
using NUnit.Framework;


namespace Data_DrivenNUnitTests
{
    public class Tests
    {
        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "9000", "Varna")]
        public void TestZippopotamus(string countryCode,string zipCode,  string expectedPlace)
        {
            //Arrange
            var restClient = new RestClient("https://api.zippopotam.us");
            var httpRequest = new RestRequest(countryCode + "/" + zipCode);

            // Act
            var httpResponce = restClient.Execute(httpRequest);
            var location = Newtonsoft.Json.JsonConvert.DeserializeObject<Location>(httpResponce.Content);

            //Assert
            StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);
        }
    }
}