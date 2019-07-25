using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository.Interfaces;

namespace ProAgil.Repository
{
    public class EventoRepository : ProAgilRepository, IEventoRepository
    {        
        private readonly ProAgilContext _context;

        public EventoRepository(ProAgilContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Evento> GetAsyncById(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                            .Include(c => c.Lotes)
                            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento)
                         .Where(w => w.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Evento>> GetAllAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                            .Include(c => c.Lotes)
                            .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento);

            return await query.ToListAsync();
        }

        public async Task<List<Evento>> GetAllAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                                .Include(c => c.Lotes)
                                .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.DataEvento)
                         .Where(w => w.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToListAsync();
        }
    }
}