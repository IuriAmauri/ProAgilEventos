using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.Repository.Interfaces;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IEventoRepository _eventoRepository;

        public EventosController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _eventoRepository.GetAllAsync(includePalestrantes: true));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                return Ok(await _eventoRepository.GetAsyncById(eventoId, includePalestrantes: true));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                return Ok(await _eventoRepository.GetAllAsyncByTema(tema, includePalestrantes: true));
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Evento eventoModel)
        {
            try
            {
                _eventoRepository.Add(eventoModel);

                if (await _eventoRepository.SaveChangesAsync())
                    return Created($"/api/eventos/{eventoModel.Id}", eventoModel);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, [FromBody] Evento eventoModel)
        {
            try
            {
                var evento = await _eventoRepository.GetAsyncById(eventoId, includePalestrantes: false);
                
                if (evento == null)
                    return NotFound();

                _eventoRepository.Update(eventoModel);

                if (await _eventoRepository.SaveChangesAsync())
                    return Created($"/api/eventos/{eventoModel.Id}", eventoModel);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var evento = await _eventoRepository.GetAsyncById(eventoId, includePalestrantes: false);
                
                if (evento == null)
                    return NotFound();

                _eventoRepository.Delete(evento);

                if (await _eventoRepository.SaveChangesAsync())
                    return Ok();
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
            }

            return BadRequest();
        }
    }
}