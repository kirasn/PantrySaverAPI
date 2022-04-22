using PantrySaver.Models;

namespace BussinessManagement.ItemManagement
{
    public interface IItemManagementBL
    {
        Task<List<Item>> GetItems(string userID);
        Task<Item> GetItem(string itemID);
        Task<Item> AddNewItem(Item item);
    }
}