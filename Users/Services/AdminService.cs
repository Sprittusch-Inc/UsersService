using Users.Models;
using MongoDB.Driver;
using System;

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
    public async Task<IResult> CreateAdmin(Admin a)
    {
        _logger.LogInformation("Creating Admin");

        try
        {

            if (a.Email != null && a.UserName != null && a.Password != null)
            {
                _logger.LogInformation("Processing password...");
                string hashedPassword = hashing.HashString(a.Password, out var passwordSalt);

                _logger.LogInformation("Attempting to insert admin in database...");
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
                    _logger.LogInformation($"Successfully inserted admin with the email {a.Email} in the database");
            }
            else
            {
                throw new Exception("A user must contain a unique email, username and password");
            }

            return Results.Ok();
        }
        catch(Exception ex)
       {
        throw ex;
        _logger.LogError(ex.Message);
        return Results.BadRequest();
       } 


    }

    public async Task DeleteAdmin(Admin a)
{
    _logger.LogInformation($"Deleting Admin with email: {a.Email}");
    
    try
    {
            _logger.LogInformation("Attempting to delete admin in database...");
            await _collection.DeleteOneAsync(x => x.Email == a.Email);
            _logger.LogInformation($"Successfully deleted admin with the email: {a.Email}");
    }
    catch(Exception ex)
    {
        _logger.LogError(ex.Message);
        throw;
    }
}

}