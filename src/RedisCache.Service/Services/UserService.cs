namespace RedisCache.Service.Services;
public class UserService(
    ICacheRepository<User> userCache,
    ICacheRepository<List<User>> allUsersCache, 
    ILogger<UserService> logger)
    : IUserService
{
    private const string AllUsersCacheKey = "all_users_list";
        
    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        User newUser = new(_nextId++, userDto.Username, userDto.Email);
        Database.Add(newUser.Id, newUser);
            
        logger.LogInformation("User created in DB with ID {Id}", newUser.Id);
            
        string idString = newUser.Id.ToString();
        await userCache.SetAsync(idString, newUser);
            
        await allUsersCache.RemoveAsync(AllUsersCacheKey); 
            
        logger.LogInformation("All Users cache list invalidated.");
            
        return newUser;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        string idString = id.ToString(); 
        var user = await userCache.GetAsync(idString); 

        if (user is not null)
        {
            logger.LogInformation("Cache HIT for user {Id}", id);
            return user;
        }
            
        logger.LogWarning("Cache MISS for user {Id}. Accessing DB.", id);
        await Task.Delay(200); 
            
        if (!Database.TryGetValue(id, out var userFromDb))
            throw new KeyNotFoundException($"User with ID {id} not found.");
            
        await userCache.SetAsync(idString, userFromDb); 
        logger.LogInformation("Cache updated for user {Id}", id);

        return userFromDb;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var users = await allUsersCache.GetAsync(AllUsersCacheKey);
            
        if (users is not null)
        {
            logger.LogInformation("Cache HIT for ALL USERS list.");
            return users;
        }
            
        logger.LogWarning("Cache MISS for ALL USERS list. Accessing DB.");
        await Task.Delay(400); 
            
        var usersFromDb = Database.Values.ToList();

        await allUsersCache.SetAsync(AllUsersCacheKey, usersFromDb, TimeSpan.FromMinutes(5));
            
        logger.LogInformation("Cache updated for ALL USERS list.");

        return usersFromDb;
    }

    public async Task<User> UpdateUserAsync(int id, CreateUserDto userDto)
    {
        if (!Database.ContainsKey(id))
            throw new KeyNotFoundException($"User with ID {id} not found for update.");

        var updatedUser = new User(id, userDto.Username, userDto.Email);
        Database[id] = updatedUser; 

        logger.LogInformation("User {Id} updated in DB.", id);
        
        await userCache.RemoveAsync(id.ToString());
        await allUsersCache.RemoveAsync(AllUsersCacheKey);
            
        logger.LogInformation("Individual user key and All Users cache list invalidated.");
            
        return updatedUser;
    }

    public async Task DeleteUserAsync(int id)
    {
        if (!Database.Remove(id))
            throw new KeyNotFoundException($"User with ID {id} not found for deletion.");

        logger.LogInformation("User {Id} deleted from DB.", id);
        
        await userCache.RemoveAsync(id.ToString());
        await allUsersCache.RemoveAsync(AllUsersCacheKey);
            
        logger.LogInformation("Individual user key and All Users cache list invalidated after deletion.");
    }
        
    private static readonly Dictionary<int, User> Database;
    private static int _nextId;

    static UserService()
    {
        string seedPath = Path.Combine(AppContext.BaseDirectory, "UserSeedData.json");

        if (File.Exists(seedPath))
        {
            string jsonString = File.ReadAllText(seedPath);
            var users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? [];
            
            Database = users.ToDictionary(u => u.Id, u => u);
            _nextId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
        }
        else
        {
            Database = new Dictionary<int, User>();
            _nextId = 1;
        }
    }
}