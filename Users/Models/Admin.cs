using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Models;

public class Admin
{
    [BsonElement("_id")]
    [BsonId]

public ObjectId Id{get; set;}
public int AdminId {get; set;}
public string? Email{get; set;}
public string? UserName{get; set;}
public string? Password{get; set;}
public Byte[]? Salt{get; set;}
public string? Role{get; set;} = "Admin";

}