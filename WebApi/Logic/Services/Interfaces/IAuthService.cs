namespace WebApi.Logic.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> AdminLoginAsync(string userName, string password);
        Task<string> EmployeeLoginAsync(string userName, string password);
    }
}