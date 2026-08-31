using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.DTOs.ConferenceRoom;
using ConferenceBooking.Application.DTOs.Search;
using FluentValidation;

namespace ConferenceBooking.API.Controllers
{
    /// <summary>CRUD конференц-залів та пошук доступних.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConferenceRoomsController : ControllerBase
    {
        private readonly IConferenceRoomService _roomService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateConferenceRoomDto> _createValidator;
        private readonly IValidator<UpdateConferenceRoomDto> _updateValidator;
        private readonly IValidator<AvailableRoomSearchDto> _searchValidator;

        public ConferenceRoomsController(
            IConferenceRoomService roomService, 
            IMapper mapper,
            IValidator<CreateConferenceRoomDto> createValidator,
            IValidator<AvailableRoomSearchDto> searchValidator,
            IValidator<UpdateConferenceRoomDto> updateValidator)
        {
            _roomService = roomService;
            _mapper = mapper;
            _createValidator = createValidator;
            _searchValidator = searchValidator;
            _updateValidator = updateValidator;
        }

        // GET api/conferencerooms
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            var response = _mapper.Map<IEnumerable<ConferenceRoomResponseDto>>(rooms);
            return Ok(response);
        }

        // GET api/conferencerooms/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<ConferenceRoomResponseDto>(room);
            return Ok(response);
        }

        // GET api/conferencerooms/available?date=...&startTime=...&endTime=...&capacity=...
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] AvailableRoomSearchDto search)
        {
            var validation = await _searchValidator.ValidateAsync(search);
            if(!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
            var startDateTime = search.Date + search.StartTime;
            var endDateTime = search.Date + search.EndTime;
            
            var rooms = await _roomService.GetAvailableRoomsAsync(startDateTime, endDateTime, search.Capacity);

            var response = _mapper.Map<IEnumerable<ConferenceRoomResponseDto>>(rooms);
            return Ok(response);
        }

        // POST api/conferencerooms
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConferenceRoomDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var room = await _roomService.CreateAsync(
                dto.Name, dto.Capacity, dto.BaseHourlyRate, dto.ServiceIds);

            var response = _mapper.Map<ConferenceRoomResponseDto>(room);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, response);
        }

        // PUT api/conferencerooms/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateConferenceRoomDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var room = await _roomService.UpdateAsync(id, dto.Name, dto.Capacity, dto.BaseHourlyRate, dto.ServiceIds);

            var response = _mapper.Map<ConferenceRoomResponseDto>(room);
            return Ok(response);
        }

        // DELETE api/conferencerooms/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roomService.DeleteAsync(id);
            return NoContent();
        }
    }
}
