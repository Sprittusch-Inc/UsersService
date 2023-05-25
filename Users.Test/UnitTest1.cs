using Users.Models;
using Users.Controllers;
using Users.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Microsoft.AspNetCore.Mvc;


namespace Users.Test;

public class Tests
{
    private ILogger<UserController> _logger = null!;
    private IConfiguration _configuration = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<UserController>>().Object;

        var myConfiguration = new Dictionary<string, string?>
        {
            {"UserServiceBrokerHost", "http://testhost.local"}
        };

         _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        
    }

    [Test]
    public void TestUserEndPoint_ValidUser()
    {  
        
       
         

        
    }
}