using AutoMapper;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;

namespace CodingChallenge.Service.MappingProfiles
{
    /// <summary>
    /// The transaction mapping profile.
    /// </summary>
    public class TransactionMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionMappingProfile"/> class.
        /// </summary>
        public TransactionMappingProfile()
        {
            // Not a Big Fan of AutoMapper, as I prefer Factory Pattern. Only Added this as request
            CreateMap<TransactionDataModel, TransactionDto>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TransactionAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TransactionCreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType));
        }
    }
}
