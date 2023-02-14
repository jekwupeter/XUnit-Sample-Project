using Microsoft.EntityFrameworkCore;
using TimiTest.Data;
using TimiTest.Models;

namespace TimiTest.Repositories
{
    
    public class KitchenRepository : IKitchenRepository
    {
        private readonly KitchenDbContext _db;

        public KitchenRepository(KitchenDbContext db)
        {
            _db = db;
        }
        public async void AddItem(Kitchen testItem)
        {
            _db.Add(testItem);

            await _db.SaveChangesAsync();
        }

        public async Task<List<Kitchen>> GetAll()
        {
            return await _db.Kitchens.ToListAsync();
        }

        public async Task<Kitchen> GetById(int id)
        {
            var item = await _db.Kitchens.FirstOrDefaultAsync(t => t.Id == id);
            return item;
        }
    }
}
