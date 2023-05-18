using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using System.Text;
using Microsoft.AspNetCore.Authorization;
using Users.Models;
using System.Security.Cryptography;
using MongoDB.Driver;
using Users;

namespace Users.Controllers;

[ApiController]
[Route("[controller]")]


public class UserController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly ILogger<UserController> _logger;
    protected static IMongoClient _client;
    protected static IMongoDatabase _db;
    private Vault vault = new();
    
    


    public UserController(ILogger<UserController> logger, IConfiguration config)
    {
        string cons = vault.GetSecret("dbconnection", "constring").Result;
        string constring = vault.secret;
        Console.WriteLine(cons);
        Console.WriteLine();

        //"mongodb://admin:1234@localhost:27018/?authsource=admin"
         
        _config = config;
        _logger = logger;
        _client = new MongoClient(cons);
        _db = _client.GetDatabase("user");
    }

    

    const int keySize = 64;
    const int iterations = 350000;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
    string HashString(string s, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(keySize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(s),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        return Convert.ToHexString(hash);
    }

    


    // Initialize settings. You can also set proxies, custom delegates etc.here.
    

        /*
        [HttpGet]
        */
        [AllowAnonymous]
        [HttpPost("createUser")]
    public async Task CreateUser(User u)
    {
        

        _logger.LogInformation("Creating User");

        string hashedPassword = HashString(u.Password, out var passwordSalt);
        string hashedCardN = HashString(u.CardN, out var cardSalt);
        var collection = _db.GetCollection<User>("users");
        await collection.InsertOneAsync(
            new Models.User
            {
                Email = u.Email,
                UserName = u.UserName,
                Password = hashedPassword,
                CardN = hashedCardN,
                IsBusiness = u.IsBusiness,
                Cvr = u.Cvr,
                Iban = u.Iban
            }
                );
    }



    [Authorize]
    [HttpPost("createAdmin")]
    public async Task CreateAdmin(Admin a)
    {
        _logger.LogInformation("Creating User");

        string hashedPassword = HashString(a.Password, out var passwordSalt);

        var collection = _db.GetCollection<Admin>("admin");
        await collection.InsertOneAsync(
            new Models.Admin
            {
                Email = a.Email,
                UserName = a.UserName,
                Password = hashedPassword,
                AdminId = a.AdminId
            }
                );
    }

    [AllowAnonymous]
    [HttpPut("deleteUser")]
    public async Task DeleteUser(User u)
    {
        _logger.LogInformation("Creating User");


        var collection = _db.GetCollection<User>("users");
        collection.DeleteOneAsync(x => x.Email == u.Email);

    }

    /*
    [HttpDelete]
    [HttpPut]
    */

}

