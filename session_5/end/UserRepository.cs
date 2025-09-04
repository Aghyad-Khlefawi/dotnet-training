namespace training;

public interface IUserRepository
{
  Task<bool> IsUserValid(string username, string password);
}

public class MockUserRepository : IUserRepository
{

  private readonly Dictionary<string, string> _users = new()
  {
    {"admin","123"}
  };

  public Task<bool> IsUserValid(string username, string password)
  {
    return Task.FromResult(_users.TryGetValue(username.ToLower(), out var userPass) && userPass == password);
  }
}
