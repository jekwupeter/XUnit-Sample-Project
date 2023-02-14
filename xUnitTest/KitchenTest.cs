using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography.X509Certificates;
using TimiTest.Controllers;
using TimiTest.Data;
using TimiTest.Models;
using TimiTest.Repositories;

namespace xUnitTest
{
    public class KitchenTest
    {
        public KitchenTest()
        {
            /*
           IDictionary<string, string> appsettings = new Dictionary<string, string>()
           {
               { "URLNAME:url", "http://10.0.0.0:1/api/" }
           };

           IConfiguration _config = new ConfigurationBuilder().AddInMemoryCollection(appsettings).Build();
           */
            // unit test service with isolated config file
            
        }

        [Fact]
        public async Task GetAllTestItemsAsync_ReturnsOkResult()
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();

            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            // Act
            var result = await controller.GetAllTestItemsAsync();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllTestItemsAsync_Returns200()
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();
            
            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data

            var controller = new KitchenController(kitchenRepositoryMock.Object);
            
            // Act
            var item = await controller.GetAllTestItemsAsync() as OkObjectResult;

            // Assert
            Assert.Equal(200, item.StatusCode);
            
        }

        [Fact]
        public async void GetAllTestItems_ReturnsCorrectNumberOfValues()
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();

            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            // Act
            var item = await controller.GetAllTestItemsAsync() as OkObjectResult;
            var items = (List<Kitchen>)item.Value;
                
            // Assert
            Assert.Equal(2, items.Count);
            // assert.equal(expected, actualvalue)

        }

        [Fact]
        public async Task AddItem_ReturnOkResult()
        {
            // Arrange
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();
            var controller = new KitchenController(kitchenRepositoryMock.Object);
            var testModel = new Kitchen() { Id = 9, Name = "Test", Code = "Add" };

            // Act
            var result = controller.AddTestItem(testModel);

            //Assert
            Assert.IsType<OkResult>(result); //Okresult becuase no object is returned by IActionresult
            kitchenRepositoryMock.Verify(x => x.AddItem(testModel), Times.Once());
        }

        /*
        [Fact]
        public async Task GetTestItem_ReturnsNotFoundResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<KitchenDbContext>().
                UseInMemoryDatabase(databaseName: "GetById_ReturnsNull")
                .Options;

            var dbContext = new KitchenDbContext(options);
            var kitchenRepositoryMock = new KitchenRepository(dbContext);

            // Act
            Kitchen result = null;
            await Assert.ThrowsAsync<Exception>(async () => await kitchenRepositoryMock.GetById(null));
        }*/

        [Fact]
        public async void GetTestItemAsync_ReturnCorrectValueUsingId()
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();

            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data
            kitchenRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(GetTestSamples()[0]);

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            // Act
            var item = await controller.GetTestItemAsync(1) as OkObjectResult;
            Console.WriteLine(item);
            var modelItem = ((Kitchen)item.Value);
            string modelItemName = modelItem.Name;
            // Assert
            Assert.Equal(testSamples[0].Name, modelItemName);

        }

        [Theory]
        [MemberData(nameof(KitchenShareData.KitchenData), MemberType = typeof(KitchenShareData))]
        public async void GetTestItemAsync_ReturnsCorrectObjectValuesUsingId(int kitchenId, Kitchen expectedKitchen)
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();

            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data
            kitchenRepositoryMock.Setup(repo => repo.GetById(kitchenId)).ReturnsAsync(GetTestSamples()[0]);

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            // Act
            var item = await controller.GetTestItemAsync(kitchenId) as OkObjectResult;
            
            var modelItem = ((Kitchen)item.Value);
            //string modelItemName = modelItem.Name;

            // Assert
            Assert.Equal(expectedKitchen.Id, modelItem.Id);
        }

        [Theory]
        [InlineData(1, 1)]
        public async void GetTestItemAsync_ReturnsCorrectObjectId(int kitchenId, int expectedKitchenId)
        {
            var testSamples = GetTestSamples();
            // mock: testing dependency in isolation
            var kitchenRepositoryMock = new Mock<IKitchenRepository>();

            kitchenRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(GetTestSamples()); // setups a mock repo with data
            kitchenRepositoryMock.Setup(repo => repo.GetById(kitchenId)).ReturnsAsync(GetTestSamples()[0]);

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            // Act
            var item = await controller.GetTestItemAsync(kitchenId) as OkObjectResult;

            var modelItem = ((Kitchen)item.Value);
            //string modelItemName = modelItem.Name;

            // Assert
            Assert.Equal(expectedKitchenId, modelItem.Id);
        }

        [Theory]
        [InlineData(9, "Test", "Go")]
        public async Task GetTestItemAsync_ReturnsCorrectObjectFromExternalService(int kitchenId, string expectedKitchenName, string expectedKitchenCode)
        {
            // Arrange

            var expectedResult = new Kitchen { Id = kitchenId, Name = expectedKitchenName, Code = expectedKitchenCode };

            var kitchenRepositoryMock = new Mock<IKitchenRepository>();
            kitchenRepositoryMock.Setup(x => x.GetById(kitchenId)).ReturnsAsync(expectedResult);

            var controller = new KitchenController(kitchenRepositoryMock.Object);

            //Act
            var result = await controller.GetTestItemAsync(kitchenId) as OkObjectResult;

            // assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result.Value);
        }

        // fixtures for DI
        private List<Kitchen> GetTestSamples()
        {
            var samples = new List<Kitchen>();

            samples.Add(new Kitchen() { Id = 1, Name="Island", Code="002" });
            samples.Add(new Kitchen() { Id = 2, Name = "Parallel", Code = "003" });

            return samples;
        }
    }
}