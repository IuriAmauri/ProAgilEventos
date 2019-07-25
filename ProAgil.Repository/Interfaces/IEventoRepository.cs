using System.Collections.Generic;
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository.Interfaces
{
    public interface IEventoRepository : IProAgilRepository
    {
        Task<List<Evento>> GetAllAsync(bool includePalestrantes = false);
        Task<List<Evento>> GetAllAsyncByTema(string tema, bool includePalestrantes = false);
        Task<Evento> GetAsyncById(int eventoId, bool includePalestrantes = false);
    }
}