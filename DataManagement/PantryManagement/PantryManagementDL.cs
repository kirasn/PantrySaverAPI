using Microsoft.EntityFrameworkCore;
using PantrySaver.Models;

namespace DataManagement.PantryManagement
{
    public class PantryManagementDL : IPantryManagementDL
    {
        private readonly PantrySaverContext _context;

        public PantryManagementDL(PantrySaverContext context)
        {
            _context = context;
        }

        public async Task<Pantry> CreateNewPantry(string userName, Pantry pantry)
        {
            var userFromDB = await _context.Users.FirstOrDefaultAsync(p => p.UserName.Equals(userName));

            await _context.Database.BeginTransactionAsync();
            await _context.Pantries.AddAsync(pantry);
            PantryOwn _newPantryOwn = new PantryOwn()
            {
                PantryOwnId = Guid.NewGuid().ToString(),
                PantryId = pantry.PantryId,
                UserId = userFromDB!.Id,
                Role = "Owner"
            };
            _context.PantryOwns.Add(_newPantryOwn);
            await _context.Database.CommitTransactionAsync();
            await _context.SaveChangesAsync();

            return pantry;
        }

        public async Task<List<PantryOwn>> GetPantries(string userID)
        {
            return await _context.PantryOwns.Where(p => p.UserId.Equals(userID)).
                                    Select(p =>
                                        new PantryOwn()
                                        {
                                            PantryId = p.PantryId,
                                            Role = p.Role,
                                            Pantry = new Pantry()
                                            {
                                                PantryId = p.Pantry.PantryId,
                                                PantryName = p.Pantry.PantryName,
                                                Location = p.Pantry.Location,
                                            }
                                        }
                                    ).ToListAsync();
        }

        public async Task<Pantry> GetPantryDetails(string userID, string pantryId)
        {
            List<PantryOwn> _pantryOwn = await GetPantries(userID);

            Pantry _pantry = await _context.Pantries.FirstOrDefaultAsync(p => p.PantryId.Equals(pantryId));

            if (_pantryOwn.FirstOrDefault(p => p.PantryId.Equals(_pantry.PantryId)) == null)
                throw new Exception("You don't have access to this pantry!");

            return _pantry;
        }
    }
}