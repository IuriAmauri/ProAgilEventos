using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Domain
{
    public class EventoDto {
        public int? Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        [Required]
        public string Tema { get; set; }
        [Range(1, 10000)]
        public int QtdPessoas { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        [EmailAddress]
        public string Email { get; set; } 
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}