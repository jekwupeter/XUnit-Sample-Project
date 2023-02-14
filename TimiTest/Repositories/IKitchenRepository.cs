using TimiTest.Models;

namespace TimiTest.Repositories
{
    public interface IKitchenRepository
    {
        Task<List<Kitchen>> GetAll();
        Task<Kitchen> GetById(int id);
        void AddItem(Kitchen testItem);
    }
}
