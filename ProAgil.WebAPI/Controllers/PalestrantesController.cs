using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository.Interfaces;

namespace ProAgil.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestranteRepository _palestranteRepository;

        public PalestrantesController(IPalestranteRepository palestranteRepository)
        {
            _palestranteRepository = palestranteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _palestranteRepository.GetAllAsync(includeEventos: true));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }
        }

        [HttpGet("{palestranteId}")]
        public async Task<IActionResult> Get(int palestranteId)
        {
            try
            {
                return Ok(await _palestranteRepository.GetAsyncById(palestranteId, includeEventos: true));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }
        }

        [HttpGet("getByNome/{nome}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                return Ok(await _palestranteRepository.GetAllAsyncByNome(tema, includePalestrantes: true));
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Palestrante palestranteModel)
        {
            try
            {
                _palestranteRepository.Add(palestranteModel);

                if (await _palestranteRepository.SaveChangesAsync())
                    return Created($"/api/palestrantes/{palestranteModel.Id}", palestranteModel);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante palestranteModel)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetAsyncById(palestranteId, includeEventos: false);
                
                if (palestrante == null)
                    return NotFound();

                _palestranteRepository.Update(palestranteModel);

                if (await _palestranteRepository.SaveChangesAsync())
                    return Created($"/api/palestrantes/{palestranteModel.Id}", palestranteModel);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId, Palestrante palestranteModel)
        {
            try
            {
                var palestrante = await _palestranteRepository.GetAsyncById(palestranteId, includeEventos: false);
                
                if (palestrante == null)
                    return NotFound();

                _palestranteRepository.Delete(palestranteModel);

                if (await _palestranteRepository.SaveChangesAsync())
                    return Ok();
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de dados falhou: {e.Message}");
            }

            return BadRequest();
        }
    }
}