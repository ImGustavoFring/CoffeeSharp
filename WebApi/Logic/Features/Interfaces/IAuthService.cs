namespace WebApi.Logic.Features.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string userName, string password);
    }
}