using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository.Interfaces;

namespace ProAgil.Repository
{    
    public class PalestranteRepository : ProAgilRepository, IPalestranteRepository
    {
        private readonly ProAgilContext _context;

        public PalestranteRepository(ProAgilContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Palestrante> GetAsyncById(int eventoId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                            .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(c => c.Nome)
                         .Where(w => w.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Palestrante>> GetAllAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                            .Include(c => c.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderBy(c => c.Nome);

            return await query.ToListAsync();
        }

        public async Task<List<Palestrante>> GetAllAsyncByNome(string nome, bool includePalestrantes = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                                .Include(c => c.RedesSociais);

            if (includePalestrantes)
            {
                query = query.Include(pe => pe.PalestranteEventos)
                             .ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(c => c.Nome)
                         .Where(w => w.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToListAsync();
        }
    }
}