using System.Collections.Generic;

namespace ProAgil.Domain
{
	public class PalestranteDto {
		public string Nome { get; set; }
		public string MiniCurriculo { get; set; }
		public string ImagemUrl { get; set; }
		public string Email { get; set; }
		public List<RedeSocialDto> RedesSociais { get; set; }
	}
}