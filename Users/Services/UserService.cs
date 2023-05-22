using Users.Models;
using MongoDB.Driver;

namespace Users.Services;

public class UserService
{
private readonly ILogger _logger;
private readonly IMongoCollection<User> _collection;
private Hashing hashing = new();

public UserService(ILogger logger, IMongoCollection<User> collection)
{
_collection = collection;
_logger = logger;
}
public async Task CreateUser(User u)
{

        _logger.LogInformation("Creating User");

     
       try{

        if(u.Email != null && u.UserName != null && u.Password != null)
        {
        string hashedPassword = hashing.HashString(u.Password, out var passwordSalt);    
        //var collection = _db.GetCollection<User>("users");
        await _collection.InsertOneAsync(
            new Models.User
            {
                Email = u.Email,
                UserName = u.UserName,
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsBusiness = u.IsBusiness,
                Cvr = u.Cvr
            }
        
        
        );
        
        }

        else
        {
            throw new Exception("A user must contain a unique email, username and password");
        }
       }
       catch(Exception ex)
       {
        _logger.LogError(ex.Message);
        
       }   

} 

public async Task UpdateUser(User u)
{
    _logger.LogInformation("Updating User");

    try {

    if(u.Iban != null)
    {
    string hashedIban = hashing.HashString(u.Iban, out var ibanSalt);
    
        var filter = Builders<User>.Filter.Eq("Email", u.Email);
        var update = Builders<User>.Update.Set("Iban", hashedIban); 
        var updateSalt = Builders<User>.Update.Set("IbanSalt", ibanSalt);
        
        await _collection.UpdateOneAsync(filter, update);
        await _collection.UpdateOneAsync (filter, updateSalt);
    }

    if(u.CardN != null)
    {
        string hashedCard = hashing.HashString(u.CardN, out var cardSalt);
        var filter = Builders<User>.Filter.Eq("Email", u.Email);
        var update = Builders<User>.Update.Set("CardN", hashedCard); 
        var updateSalt = Builders<User>.Update.Set("CardSalt", cardSalt);
        
        await _collection.UpdateOneAsync(filter, update);
        await _collection.UpdateOneAsync (filter, updateSalt);
    }

    else
    {
        throw new Exception("Please enter either a valid Iban number, or card number");
    }
    }

    catch(Exception ex)
    {
        _logger.LogError(ex.Message);
    }    
}

public async Task DeleteUser(User u)
{
    _logger.LogInformation("Deleting User");
    
    await _collection.DeleteOneAsync(x => x.Email == u.Email);
}

public async Task<bool> IsUnique(User u)
{
   return false;

}

                
}