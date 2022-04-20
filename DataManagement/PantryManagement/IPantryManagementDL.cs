using PantrySaver.Models;

namespace DataManagement.PantryManagement
{
    public interface IPantryManagementDL
    {
        Task<List<PantryOwn>> GetPantries(string userID);
        Task<Pantry> CreateNewPantry(string userName, Pantry pantry);
        Task<Pantry> GetPantryDetails(string userID, string pantryId);
    }
}