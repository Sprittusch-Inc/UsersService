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
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;


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
    public void TestCreateUser_EmailNull()
    {  
        
        //var stubrepo = new Mock<IMongoCollection<User>>;
        var stubRepo = new Mock<IMongoCollection<User>>();
        stubRepo.Setup(svc => svc.InsertOneAsync(It.IsAny<User>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>());

        var test = new User(){Email = null, UserName = "test", Password = "test"};

        var us = new UserService(_logger, stubRepo.Object);
       
        Assert.That(() => us.CreateUser(test), Throws.Exception);

        
    }

    [Test]
    public void TestCreateUser_UserNameNull()
    {  
        
        //var stubrepo = new Mock<IMongoCollection<User>>;
        var stubRepo = new Mock<IMongoCollection<User>>();
        stubRepo.Setup(svc => svc.InsertOneAsync(It.IsAny<User>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>());

        var test = new User(){Email = "test", UserName = null, Password = "test"};

        var us = new UserService(_logger, stubRepo.Object);
       
        Assert.That(() => us.CreateUser(test), Throws.Exception);

        
    }

    [Test]
    public void TestCreateUser_PasswordNull()
    {  
        
        //var stubrepo = new Mock<IMongoCollection<User>>;
        var stubRepo = new Mock<IMongoCollection<User>>();
        stubRepo.Setup(svc => svc.InsertOneAsync(It.IsAny<User>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>());

        var test = new User(){Email = "test", UserName = "test", Password = null};

        var us = new UserService(_logger, stubRepo.Object);
       
        Assert.That(() => us.CreateUser(test), Throws.Exception);

        
    }

    [Test]
    public void TestCreateUser_ValidUser()
    {  
        
        //var stubrepo = new Mock<IMongoCollection<User>>;
        var stubRepo = new Mock<IMongoCollection<User>>();
        stubRepo.Setup(svc => svc.InsertOneAsync(It.IsAny<User>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>());

        var test = new User(){Email = "test", UserName = "test", Password = "test"};

        var us = new UserService(_logger, stubRepo.Object);
        
        
       
    Assert.DoesNotThrow(() => us.CreateUser(test));
        
    }
}