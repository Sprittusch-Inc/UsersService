using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.Models;
using MongoDB.Driver;
using Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly ILogger<UserController> _logger;
    protected static IMongoClient _client;
    protected static IMongoDatabase _db;
    private readonly UserService _uService;
    private static string? _connString;
    private Hashing hashing = new();




    public UserController(ILogger<UserController> logger, IConfiguration config)
    {
        /*
        Vault vault = new Vault(config);
        // Henter connectionstring fra vault
        string cons = vault.GetSecret("dbconnection", "constring").Result;
        */

        _config = config;
        _logger = logger;
        _connString = config["MongoConnection"];

        //starter en client med den hentede connectionstring
        _client = new MongoClient(_connString);

        //Henter user databasen fra mongodb
        _db = _client.GetDatabase("user");

        //Henter kollektion users
        var _uCollection = _db.GetCollection<User>("users");

        //Henter kollektion 
        _uService = new UserService(_logger, _uCollection);
        
        
    }

    

   
    [HttpPost("user")]
    [AllowAnonymous]
    public async Task CreateUser(User u)
    {
        await _uService.CreateUser(u);
    }

    
    [HttpGet("user")]
    [Authorize(Roles = "User")]
    public async Task GetUser(User u)
    {
        await _uService.GetUser(u);
    }

    
    [HttpPut("user")]
    [Authorize(Roles = "User")]
    public async Task UpdateUser(User u)
    {
        await _uService.UpdateUser(u);
    }

    
    [HttpDelete("user")]
    [Authorize(Roles = "User")]
    public async Task DeleteUser(User u)
    {
        await _uService.DeleteUser(u);
    }

    

    

   



}

