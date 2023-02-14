using Microsoft.AspNetCore.Mvc;
using TimiTest.Data;
using TimiTest.Models;
using TimiTest.Repositories;

namespace TimiTest.Controllers
{
    [ApiController]
    public class KitchenController : Controller
    {
        private readonly IKitchenRepository _repository;

        public KitchenController(IKitchenRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllItems")]
        public async Task<IActionResult> GetAllTestItemsAsync()
        {
            var testItems = await _repository.GetAll();

            return Ok(testItems);
        }

        [HttpGet]
        [Route("GetItem/{id}")]
        public async Task<IActionResult> GetTestItemAsync(int id)
        {
            var testItem = await _repository.GetById(id);
            
            return Ok(testItem);
        }

        [HttpPost]
        [Route("AddItem")]
        public IActionResult AddTestItem(Kitchen testModel)
        {
            _repository.AddItem(testModel);

            return Ok();
        }
    }
}
