using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Users.Models;


public class User{
    [BsonElement("_id")]
    [BsonId]

public ObjectId Id{get; set;}
public string? Email{get; set;}
public string? UserName{get; set;}
public string? Password{get; set;}
public string? Role{get; set;} = "User";
public Byte[]? CardSalt{get; set;}
public Byte[]? PasswordSalt{get;set;}
public string? CardN{get; set;}
public bool IsBusiness{get; set;}
public string? Iban{get; set;}
public byte[]? IbanSalt{get; set;}
public string? Cvr{get; set;}






}