using Microsoft.AspNetCore.Mvc;
using Otavio.DesafioRotaViagem.Api.Dtos;
using Otavio.DesafioRotaViagem.Api.Services;

namespace Otavio.DesafioRotaViagem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;

        public RotasController(IRotaService rotaService)
        {
            _rotaService = rotaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotaDto>>> GetRotas()
        {
            var rotas = await _rotaService.GetAllAsync();
            return Ok(rotas);
        }

        [HttpPost]
        public async Task<ActionResult> AddRota([FromBody] RotaRequest request)
        {
            var rota = new RotaDto(request.Origem, request.Destino, request.Valor);
            var novaRota = await _rotaService.CreateAsync(rota);
            return CreatedAtAction(nameof(GetRotaById), new { id = novaRota.Id }, novaRota);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RotaDto>> GetRotaById(int id)
        {
            var rota = await _rotaService.GetByIdAsync(id);
            if (rota == null)
            {
                return NotFound();
            }
            return Ok(rota);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRota(int id, [FromBody] RotaRequest rotaRequest)
        {
            var rotaAtualizada = new RotaDto(rotaRequest.Origem, rotaRequest.Destino, rotaRequest.Valor);
            rotaAtualizada.Id = id;
            var rota = await _rotaService.UpdateAsync(rotaAtualizada);
            if (rota == null)
            {
                return NotFound();
            }
            return Ok(rota);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveRota(int id)
        {
            var sucesso = await _rotaService.DeleteAsync(id);
            if (!sucesso)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("consulta")]
        public async Task<ActionResult<string>> ConsultarMelhorRota([FromQuery] string origem, [FromQuery] string destino)
        {
            var melhorRota = await _rotaService.GetBestRouteAsync(origem, destino);

            var custoTotal = melhorRota.Sum(r => r.Valor);
            var caminho = string.Join(" - ", melhorRota.Select(r => r.Origem).Append(destino));
            return $"{caminho} ao custo de ${custoTotal}";            
        }

        [HttpGet("locais")]
        public async Task<ActionResult<IEnumerable<string>>> ConsultarLocais()
        {
            var locais = await _rotaService.GetLocaisAsync();
            return Ok(locais);
        }
    }
}
