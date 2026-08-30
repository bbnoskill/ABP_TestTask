using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.DTOs.Reports;

namespace ConferenceBooking.API.Controllers
{
    /// <summary>Бізнес-звіти: завантаженість залів, доходи, популярність послуг.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportsController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        // GET api/reports/room-usage?startDate=...&endDate=...
        [HttpGet("room-usage")]
        public async Task<IActionResult> GetRoomUsage(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return NotFound();
            }
            var report = await _reportService.GetRoomUsageReportAsync(startDate, endDate);

            var response = _mapper.Map<RoomUsageReportDto>(report);
            return Ok(response);
        }

        // GET api/reports/revenue?startDate=...&endDate=...
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return NotFound();
            }
            var report = await _reportService.GetRevenueReportAsync(startDate, endDate);
            
            var response = _mapper.Map<RevenueReportDto>(report);
            return Ok(response);
        }

        // GET api/reports/popular-services?startDate=...&endDate=...
        [HttpGet("popular-services")]
        public async Task<IActionResult> GetPopularServices(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return NotFound();
            }
            var report = await _reportService.GetPopularServicesReportAsync(startDate, endDate);
            var response = _mapper.Map<PopularServicesReportDto>(report);
            return Ok(response);
        }
    }
}
