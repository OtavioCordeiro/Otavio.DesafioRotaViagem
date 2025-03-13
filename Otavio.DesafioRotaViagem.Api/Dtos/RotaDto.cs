using System.Runtime.CompilerServices;

namespace Otavio.DesafioRotaViagem.Api.Dtos
{
    public class RotaDto
    {
        public RotaDto(string origem, string destino, int valor)
        {
            Origem = origem;
            Destino = destino;
            Valor = valor;
        }

        public int Id { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int Valor { get; set; }
    }
}
