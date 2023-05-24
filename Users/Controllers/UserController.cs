using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.Models;
using MongoDB.Driver;
using Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Users.Controllers;

[ApiController]
[Route("[controller]")]


public class UserController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly ILogger<UserController> _logger;
    protected static IMongoClient _client;
    protected static IMongoDatabase _db;
    private readonly UserService _uService;
    private readonly AdminService _aService;
    private Vault vault = new();
    private Hashing hashing = new();
    
    


    public UserController(ILogger<UserController> logger, IConfiguration config)
    {
        string cons = vault.GetSecret("dbconnection", "constring").Result;
        
        Console.WriteLine(cons);
        Console.WriteLine();

        //"mongodb://admin:1234@localhost:27018/?authsource=admin"
         
        _config = config;
        _logger = logger;
        _client = new MongoClient(cons);
        _db = _client.GetDatabase("user");
        var _uCollection = _db.GetCollection<User>("users");
        var _aCollection = _db.GetCollection<Admin>("admin");
        _uService = new UserService(_logger, _uCollection);
        _aService = new AdminService(_logger, _aCollection);
    }

    


    // Initialize settings. You can also set proxies, custom delegates etc.here.
    

        
        [AllowAnonymous]
        [HttpPost("createUser")]
    public async Task CreateUser(User u)
    {
        
        await _uService.CreateUser(u);
                

    }

    [AllowAnonymous]
    [HttpPut("updateUser")]
    public async Task UpdateUser(User u)
    {
        await _uService.UpdateUser(u);
    }

    [AllowAnonymous]
    [HttpPost("createAdmin")]
    public async Task CreateAdmin(Admin a)
    {
        await _aService.CreateAdmin(a);
    }

    [HttpPut("deleteUser")]
    [Authorize(Roles = "Admin")]
    
    public async Task DeleteUser(User u)
    {
        await _uService.DeleteUser(u);

    }

    

}

