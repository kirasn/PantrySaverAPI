using PantrySaver.Models;

namespace DataManagement.ItemManagement
{
    public interface IItemManagementDL
    {
        Task<List<Item>> GetItems(string userID);
        Task<Item> GetItem(string itemID);
        Task<Item> AddNewItem(Item item);
    }
}