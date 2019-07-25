using System.Collections.Generic;
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository.Interfaces
{
    public interface IPalestranteRepository : IProAgilRepository
    {
        Task<List<Palestrante>> GetAllAsync(bool includeEventos = false);
        Task<List<Palestrante>> GetAllAsyncByNome(string nome, bool includePalestrantes = false);
        Task<Palestrante> GetAsyncById(int eventoId, bool includeEventos = false);
    }    
}