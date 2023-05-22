using Users.Models;

namespace Users.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        User u = new();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}