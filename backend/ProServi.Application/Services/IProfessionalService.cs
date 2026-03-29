using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public interface IProfessionalService
{
    Task<IEnumerable<ProfessionalDto>> SearchBySpecialtyAndCityAsync(int specialtyId, string city);
    Task<ProfessionalDetailDto> GetProfessionalDetailAsync(int id);
}
