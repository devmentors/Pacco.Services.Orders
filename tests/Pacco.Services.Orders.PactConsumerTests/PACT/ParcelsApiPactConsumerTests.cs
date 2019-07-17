using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pacco.Services.Orders.Application.DTO;
using Pacco.Services.Orders.PactConsumerTests.Mocks;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace Pacco.Services.Orders.PactConsumerTests.PACT
{
    public class ParcelsApiPactConsumerTests : IClassFixture<ParcelsApiMock>
    {
        [Fact]
        public async Task Given_Valid_Parcel_Id_Parcel_Should_Be_Returned()
        {
            _mockProviderService
                .Given("Existing parcel")
                .UponReceiving("A GET request to retrieve parcel details")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/parcels/{ParcelId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    },
                    Body = new ParcelDto
                    {
                        Id = new Guid(ParcelId),
                        Name = "Product",
                        Size = "huge",
                        Variant = "weapon"
                    }
                });

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_serviceUri}/parcels/{ParcelId}");
            var json = await response.Content.ReadAsStringAsync();
            var parcel = JsonConvert.DeserializeObject<ParcelDto>(json);
            
            Assert.Equal(parcel.Id.ToString(), ParcelId);
        }
        
        #region ARRANGE

        private const string ParcelId = "c68a24ea-384a-4fdc-99ce-8c9a28feac64"; 
        
        private readonly IMockProviderService _mockProviderService;
        private readonly string _serviceUri;
        
        public ParcelsApiPactConsumerTests(ParcelsApiMock fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _serviceUri = fixture.ServiceUri;
            _mockProviderService.ClearInteractions();
        }
        
        #endregion
    }
}