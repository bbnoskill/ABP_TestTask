using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.DTOs.Booking;

namespace ConferenceBooking.API.Controllers
{
    /// <summary>Створення, перегляд та скасування бронювань.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookingDto> _createValidator;

        public BookingsController(
            IBookingService bookingService,
            IMapper mapper,
            IValidator<CreateBookingDto> createValidator)
        {
            _bookingService = bookingService;
            _mapper = mapper;
            _createValidator = createValidator;
        }

        // GET api/bookings/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            var response = _mapper.Map<BookingResponseDto>(booking);
            return Ok(response);
        }

        // POST api/bookings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var booking = await _bookingService.CreateBookingAsync(
                dto.RoomId, dto.StartDateTime, dto.EndDateTime, dto.ServiceIds);

            var response = _mapper.Map<BookingResponseDto>(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, response);
        }

        // DELETE api/bookings/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _bookingService.CancelBookingAsync(id);
            return NoContent();
        }
    }
}
