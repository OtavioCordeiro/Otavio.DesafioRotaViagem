using Otavio.DesafioRotaViagem.Api.Dtos;

namespace Otavio.DesafioRotaViagem.Api.Services
{
    public interface IRotaService
    {
        Task<RotaDto> CreateAsync(RotaDto rotaDto);
        Task<RotaDto> GetByIdAsync(int id);
        Task<IEnumerable<RotaDto>> GetAllAsync();
        Task<RotaDto> UpdateAsync(RotaDto rotaDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<RotaDto>> GetBestRouteAsync(string origem, string destino);
        Task<IEnumerable<string>> GetLocaisAsync();
    }
}
