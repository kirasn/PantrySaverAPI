using Microsoft.EntityFrameworkCore;
using PantrySaver.Models;

namespace DataManagement.ItemManagement
{
    public class ItemManagementDL : IItemManagementDL
    {
        private readonly PantrySaverContext _context;

        public ItemManagementDL(PantrySaverContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetItems(string userID)
        {
            return await _context.Items.Where(p => p.IsCustom.Equals(false) || p.UserId.Equals(userID)).OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<Item> GetItem(string itemID)
        {
            return await _context.Items.FirstOrDefaultAsync(p => p.ItemId.Equals(itemID));
        }

        public async Task<Item> AddNewItem(Item item)
        {
            item.ItemId = Guid.NewGuid().ToString();
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }
    }
}