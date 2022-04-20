using DataManagement.PantryManagement;
using PantrySaver.Models;

namespace BussinessManagement.PantryManagement
{
    public class PantryManagementBL : IPantryManagementBL
    {
        private readonly IPantryManagementDL _repo;

        public PantryManagementBL(IPantryManagementDL repo)
        {
            _repo = repo;
        }

        public async Task<Pantry> CreateNewPantry(string userName, Pantry pantry)
        {
            return await _repo.CreateNewPantry(userName, pantry);
        }

        public async Task<List<PantryOwn>> GetPantries(string userID)
        {
            List<PantryOwn> _listPantryOwn = await _repo.GetPantries(userID);
            return _listPantryOwn;
        }

        public async Task<Pantry> GetPantryDetails(string userID, string pantryId)
        {
            try
            {
                return await _repo.GetPantryDetails(userID, pantryId);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}