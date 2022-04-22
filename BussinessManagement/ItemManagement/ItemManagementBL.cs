using DataManagement.ItemManagement;
using PantrySaver.Models;

namespace BussinessManagement.ItemManagement
{
    public class ItemManagementBL : IItemManagementBL
    {
        private readonly IItemManagementDL _repo;

        public ItemManagementBL(IItemManagementDL repo)
        {
            _repo = repo;
        }

        public async Task<Item> AddNewItem(Item item)
        {
            return await _repo.AddNewItem(item);
        }

        public async Task<Item> GetItem(string itemID)
        {
            return await _repo.GetItem(itemID);
        }

        public async Task<List<Item>> GetItems(string userID)
        {
            return await _repo.GetItems(userID);
        }
    }
}