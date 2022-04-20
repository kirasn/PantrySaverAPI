using PantrySaver.Models;

namespace BussinessManagement.PantryManagement
{
    public interface IPantryManagementBL
    {
        Task<List<PantryOwn>> GetPantries(string userID);
        Task<Pantry> CreateNewPantry(string userName, Pantry pantry);
        Task<Pantry> GetPantryDetails(string userID, string pantryId);
    }
}