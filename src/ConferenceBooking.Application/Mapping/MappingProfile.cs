using AutoMapper;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Application.DTOs.Booking;
using ConferenceBooking.Application.DTOs.ConferenceRoom;
using ConferenceBooking.Application.DTOs.Service;
using ConferenceBooking.Application.DTOs.Reports;
using ConferenceBooking.Application.Interfaces;

namespace ConferenceBooking.Application.Mapping
{
    /// <summary>AutoMapper профіль маппінгу Entity <-> DTO.</summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ConferenceRoom -> ConferenceRoomResponseDto
            CreateMap<ConferenceRoom, ConferenceRoomResponseDto>()
                .ForMember(dest => dest.AvailableServices,
                    opt => opt.MapFrom(src => src.AvailableServices.Select(rs => rs.Service)));

            // Service -> ServiceDto
            CreateMap<Domain.Entities.Service, ServiceDto>();

            // Booking -> BookingResponseDto
            CreateMap<Domain.Entities.Booking, BookingResponseDto>()
                .ForMember(dest => dest.RoomName,
                    opt => opt.MapFrom(src => src.ConferenceRoom.Name))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.SelectedServices,
                    opt => opt.MapFrom(src => src.SelectedServices));

            // Report models -> DTOs
            CreateMap<RoomUsageReportItem, RoomUsageReportDto>();
            CreateMap<RevenueReportResult, RevenueReportDto>();
            CreateMap<RoomRevenueItem, RoomRevenueDto>();
            CreateMap<PopularServiceReportItem, PopularServicesReportDto>();
        }
    }
}
