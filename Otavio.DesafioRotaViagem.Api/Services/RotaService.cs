using Microsoft.EntityFrameworkCore;
using Otavio.DesafioRotaViagem.Api.Contexts;
using Otavio.DesafioRotaViagem.Api.Dtos;
using Otavio.DesafioRotaViagem.Api.Entities;

namespace Otavio.DesafioRotaViagem.Api.Services
{
    public class RotaService : IRotaService
    {
        private readonly RotaDbContext _dbContext;

        public RotaService(RotaDbContext context)
        {
            _dbContext = context;
        }

        public async Task<RotaDto> CreateAsync(RotaDto rotaDto)
        {
            var rota = new Rota
            {
                Origem = rotaDto.Origem,
                Destino = rotaDto.Destino,
                Valor = rotaDto.Valor
            };

            _dbContext.Rotas.Add(rota);
            await _dbContext.SaveChangesAsync();

            rotaDto.Id = rota.Id;
            return rotaDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rota = await _dbContext.Rotas.FindAsync(id);
            if (rota == null)
            {
                return false;
            }

            _dbContext.Rotas.Remove(rota);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RotaDto>> GetAllAsync()
        {
            return await _dbContext.Rotas
                .Select(r => new RotaDto(r.Origem, r.Destino, r.Valor) { Id = r.Id })
                .ToListAsync();
        }

        public async Task<RotaDto> GetByIdAsync(int id)
        {
            var rota = await _dbContext.Rotas.FindAsync(id);
            if (rota == null)
            {
                return null;
            }

            return new RotaDto(rota.Origem, rota.Destino, rota.Valor) { Id = rota.Id };
        }

        public async Task<RotaDto> UpdateAsync(RotaDto rotaDto)
        {
            var rota = await _dbContext.Rotas.FindAsync(rotaDto.Id);
            if (rota == null)
            {
                return null;
            }

            rota.Origem = rotaDto.Origem;
            rota.Destino = rotaDto.Destino;
            rota.Valor = rotaDto.Valor;

            _dbContext.Rotas.Update(rota);
            await _dbContext.SaveChangesAsync();

            return rotaDto;
        }

        public async Task<IEnumerable<string>> GetLocaisAsync()
        {
            var rotas = await _dbContext.Rotas.ToListAsync();
            var locais = rotas.SelectMany(r => new[] { r.Origem, r.Destino })
                              .Distinct()
                              .ToList();
            return locais;
        }

        public async Task<IEnumerable<RotaDto>> GetBestRouteAsync(string origem, string destino)
        {
            var rotas = await _dbContext.Rotas.ToListAsync();
            var rotasPossiveis = new List<List<RotaDto>>();
            BuscarRotas(origem, destino, new List<RotaDto>(), rotasPossiveis, rotas);

            var melhorRota = rotasPossiveis.OrderBy(r => r.Sum(x => x.Valor)).FirstOrDefault();
            return melhorRota != null ? melhorRota.Select(r => new RotaDto(r.Origem, r.Destino, r.Valor) { Id = r.Id }) : null;
        }

        private void BuscarRotas(string atual, string destino, List<RotaDto> caminhoAtual, List<List<RotaDto>> rotasPossiveis, List<Rota> rotasCadastradas)
        {
            // Ao chegar ao destino retornamos
            if (atual == destino)
            {
                rotasPossiveis.Add(new List<RotaDto>(caminhoAtual));
                return;
            }

            // Percorre todas as rotas que partem do ponto "atual"
            foreach (var rota in rotasCadastradas.Where(r => r.Origem == atual && !caminhoAtual.Any(c => c.Id == r.Id)))
            {
                // Adiciona a rota ao caminho atual
                caminhoAtual.Add(new RotaDto(rota.Origem, rota.Destino, rota.Valor) { Id = rota.Id });
                // Chama a função recursivamente para continuar a busca a partir do próximo destino
                BuscarRotas(rota.Destino, destino, caminhoAtual, rotasPossiveis, rotasCadastradas);
                // Remove a última rota adicionada para explorar outros caminhos (Backtracking)
                caminhoAtual.RemoveAt(caminhoAtual.Count - 1);
            }
        }
    }
}