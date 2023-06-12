using Tonrich.Shared.Dtos.Account;

namespace Tonrich.Client.Shared.Services.Contracts;

public interface IAuthenticationService
{
    Task SignIn(SignInRequestDto dto);

    Task SignOut();
}
