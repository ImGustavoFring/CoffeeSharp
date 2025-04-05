namespace WebApi.Logic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string userName, string password);
    }
}