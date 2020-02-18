using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
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

        [HttpPost("upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok();
                }

                return BadRequest();
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}