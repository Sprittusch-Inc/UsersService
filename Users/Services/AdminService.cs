using Users.Models;
using MongoDB.Driver;

namespace Users.Services;

public class AdminService
{
    private readonly ILogger _logger;
    private readonly IMongoCollection<Admin> _collection;
    private Hashing hashing = new();

    public AdminService(ILogger logger, IMongoCollection<Admin> collection)
    {
        _collection = collection;
        _logger = logger;
    }
    public async Task CreateAdmin(Admin a)
    {
        _logger.LogInformation("Creating Admin");

        try
        {

            if (a.Email != null && a.UserName != null && a.Password != null)
            {

                string hashedPassword = hashing.HashString(a.Password, out var passwordSalt);


                await _collection.InsertOneAsync(
                    new Models.Admin
                    {
                        Email = a.Email,
                        UserName = a.UserName,
                        Password = hashedPassword,
                        Salt = passwordSalt,
                        AdminId = a.AdminId
                    }
                        );
            }
            else
            {
                throw new Exception("A user must contain a unique email, username and password");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }


    }

    public async Task DeleteAdmin(Admin a)
{
    _logger.LogInformation("Deleting Admin");
    
    await _collection.DeleteOneAsync(x => x.Email == a.Email);
}

}