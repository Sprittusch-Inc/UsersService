using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.Models;
using MongoDB.Driver;
using Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace Users.Controllers;

[ApiController]
[Route("api/[controller]")]


public class AdminController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly ILogger<UserController> _logger;
    protected static IMongoClient _client;
    protected static IMongoDatabase _db;
    private readonly AdminService _aService;
    private static string? _connString;
    private Hashing hashing = new();




    public AdminController(ILogger<UserController> logger, IConfiguration config)
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

        //Henter kollektion 
        var _aCollection = _db.GetCollection<Admin>("admin");
        
        _aService = new AdminService(_logger, _aCollection);
        
    }

   
    [HttpPost("admin")]
    [AllowAnonymous]
    public async Task CreateAdmin(Admin a)
    {
        await _aService.CreateAdmin(a);
    }

    
   
    [HttpDelete("Admin")]
    [AllowAnonymous]
    // [Authorize(Roles = "Admin")]
    public async Task DeleteAdmin(Admin a)
    {
        await _aService.DeleteAdmin(a);
    }

}

