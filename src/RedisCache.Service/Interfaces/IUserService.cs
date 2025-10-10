namespace RedisCache.Service.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(CreateUserDto userDto);
    Task<User> UpdateUserAsync(int id, CreateUserDto userDto);
    Task DeleteUserAsync(int id);
}