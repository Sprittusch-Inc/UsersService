using Users.Models;
using MongoDB.Driver;
using System;
using Microsoft.AspNetCore.Http;


namespace Users.Services;


public class UserService
{
    //
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

        try
        {
            //Sikrer at nødvendige felter er udfyldt, ellers smides der en exception
            if (u.Email != null && u.UserName != null && u.Password != null)
            {
                //Hasher password og smider salt    
                string hashedPassword = hashing.HashString(u.Password, out var passwordSalt);
                // Indsætter i databasen
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

                throw new Exception("A user must contain a unique email, unique username and password");


            }
        }

        catch (Exception ex)
        {
            
            _logger.LogError(ex.Message);
            throw ex;
        }




    }

    public async Task<IResult> UpdateUser(User u)
    {
        _logger.LogInformation("Updating User");

        try
        {

            //Ser om et iban nummer er blevet tastet ind
            if (u.Iban != null)
            {
                //Hasher iban nummer og smider salt    
                string hashedIban = hashing.HashString(u.Iban, out var ibanSalt);
                //Finder en user ud fra filter, laver derefter 2 opdateringer
                var filter = Builders<User>.Filter.Eq("Email", u.Email);
                var update = Builders<User>.Update.Set("Iban", hashedIban);
                var updateSalt = Builders<User>.Update.Set("IbanSalt", ibanSalt);

                await _collection.UpdateOneAsync(filter, update);
                await _collection.UpdateOneAsync(filter, updateSalt);
            }
            //Ser om et kort nummer er blevet tastet ind
            if (u.CardN != null)
            {
                //Hasher kort nummer og smider salt
                string hashedCard = hashing.HashString(u.CardN, out var cardSalt);
                //Finder en user ud fra filter, laver derefter 2 opdateringer
                var filter = Builders<User>.Filter.Eq("Email", u.Email);
                var update = Builders<User>.Update.Set("CardN", hashedCard);
                var updateSalt = Builders<User>.Update.Set("CardSalt", cardSalt);

                await _collection.UpdateOneAsync(filter, update);
                await _collection.UpdateOneAsync(filter, updateSalt);

                return Results.Ok();
            }

            //Hvis der hverken er et iban nummer eller et kort nummer bliver der kastet en exception
            else
            {
                throw new Exception("Please enter either a valid Iban number, or card number");
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Results.BadRequest();
        }
    }

    public async Task<IResult> DeleteUser(User u)
    {
        _logger.LogInformation($"Deleting User with email: {u.Email}");
        try
        {
            await _collection.DeleteOneAsync(x => x.Email == u.Email);
            return Results.Ok();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Results.BadRequest();
        }
    }

    public async Task<User> GetUser(User u)
    {
        _logger.LogInformation($"Retrieving user with email: {u.Email}");

        try
        {
            //Finder en user hvis email matcher med argumentet User u    
            var filter = Builders<User>.Filter.Eq("Email", u.Email);
            //Henter resultat ud af databasen ved hjælp af filteret
            var result = await _collection.Find(filter).SingleAsync();
            return result;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }



    }



}