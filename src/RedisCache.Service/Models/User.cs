namespace RedisCache.Service.Models;

public record User(int Id, string Username, string Email);
    
public record CreateUserDto(string Username, string Email);