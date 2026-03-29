using ProServi.Application.DTOs.Auth;

namespace ProServi.Application.Services;

public class ProfessionalService : IProfessionalService
{
    // Nota: En una implementación real, esto tendría inyección de IRepository<Professional>
    // pero como Application no debe depender de Infrastructure,
    // esta lógica se movería al layer de Infrastructure
    
    public async Task<IEnumerable<ProfessionalDto>> SearchBySpecialtyAndCityAsync(int specialtyId, string city)
    {
        throw new NotImplementedException("Este método debe ser implementado con inyección de dependencias de Infrastructure");
    }

    public async Task<ProfessionalDetailDto> GetProfessionalDetailAsync(int id)
    {
        throw new NotImplementedException("Este método debe ser implementado con inyección de dependencias de Infrastructure");
    }
}
