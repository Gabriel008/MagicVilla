using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");
            IEnumerable<Villa> VillaList = await _villaRepo.ObtenerTodos();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(VillaList));        
        } 
       

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult<VillaDto>> GetVilla(int id)
        {
            if(id==0)
            {
                _logger.LogError("Error al traer Villa con Id "+ id);
                return BadRequest(); 
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if(villa==null)
            { return NotFound(); }

            return Ok(_mapper.Map<VillaDto>(villa));        
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _villaRepo.Obtener(v=>v.Nombre.ToLower() == createDto.Nombre.ToLower()) !=null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            { return BadRequest(createDto); }


            Villa modelo = _mapper.Map<Villa>(createDto);

            await _villaRepo.Crear(modelo);

            return CreatedAtRoute("GetVilla", new {id=modelo.Id}, modelo);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            { return BadRequest(); }

            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if (villa == null)
            { return NotFound(); }

            _villaRepo.Remover(villa);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            
            if (updateDto == null || id != updateDto.Id)
            { return NotFound(); }

            Villa modelo = _mapper.Map<Villa>(updateDto);
     
            _villaRepo.Actualizar(modelo);

            return NoContent();
        }

        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            
            if (patchDto == null || id == 0)
            { return NotFound(); }

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked:false);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid) 
            { return BadRequest(ModelState); }

            Villa modelo =_mapper.Map<Villa>(villaDto);

            _villaRepo.Actualizar(modelo);

            return NoContent();
        }


    }
}
