using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public EventosController(IEventoRepository eventoRepository, IMapper mapper)
        {
            _eventoRepository = eventoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<EventoDto>>(await _eventoRepository.GetAllAsync(includePalestrantes: true)));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                return Ok(_mapper.Map<EventoDto>(await _eventoRepository.GetAsyncById(eventoId, includePalestrantes: true)));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                return Ok(_mapper.Map<EventoDto>(await _eventoRepository.GetAllAsyncByTema(tema, includePalestrantes: true)));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventoDto eventoDtoModel)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDtoModel);
                _eventoRepository.Add(evento);

                if (await _eventoRepository.SaveChangesAsync())
                    return Created($"/api/eventos/{evento.Id}", evento);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, [FromBody] Evento eventoDtoModel)
        {
            try
            {
                var evento = await _eventoRepository.GetAsyncById(eventoId, includePalestrantes: false);
                
                if (evento == null)
                    return NotFound();

                evento = _mapper.Map(eventoDtoModel, evento);
                _eventoRepository.Update(evento);

                if (await _eventoRepository.SaveChangesAsync())
                    return Accepted($"/api/eventos/{eventoDtoModel.Id}", evento);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return BadRequest();
        }
    }
}