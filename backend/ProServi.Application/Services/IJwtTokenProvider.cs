using ProServi.Domain.Entities;

namespace ProServi.Application.Services;

public interface IJwtTokenProvider
{
    string GenerateToken(User user);
}
