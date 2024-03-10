using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using AutoMapper;
using PatientTest.Core.DTOs;
using PatientTest.Core.Entities;

namespace PatientTest.Core.Utilits;

public static class AutoMapperDTO
{
    public static PatientDTO AutoMapPatient(IQueryable<Patient> queryable, List<string> givens)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<Patient, PatientDTO>()
                .ForMember(dist => dist.Gender, opt => opt.MapFrom(src => src.Gender.Name))
                .ForMember(dist => dist.Name, opt => opt.MapFrom(src => new PatientNameDTO()
                {
                    Id = src.Id,
                    Use = src.Use,
                    Family = src.Family,
                    Given = givens
                }));
        });
        return new Mapper(config).ProjectTo<PatientDTO>(queryable, null).FirstOrDefault();
    }
    
    public static PatientDTO AutoMapListPatient(Patient patient, List<string> givens, List<Genders>? genders)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<Patient, PatientDTO>()
                .ForMember(dist => dist.Gender, opt => opt.MapFrom(src => genders.Where(a => a.Id == src.GenderId).FirstOrDefault().Name))
                .ForMember(dist => dist.Name, opt => opt.MapFrom(src => new PatientNameDTO()
                {
                    Id = src.Id,
                    Use = src.Use,
                    Family = src.Family,
                    Given = givens
                }));
        });
        return new Mapper(config).Map<Patient, PatientDTO>(patient);
    }

    public static Patient AutoMapPatientDTO(AddPatientDTO patient, Guid? genderId)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<AddPatientDTO, Patient>()
                .ForMember(dist => dist.Gender, opt => opt.Ignore())
                .ForMember(dist => dist.Family, opt => opt.MapFrom(src => src.Name.Family))
                .ForMember(dist => dist.Use, opt => opt.MapFrom(a => a.Name.Use))
                .ForMember(dist => dist.GenderId, opt => opt.MapFrom(a => genderId));
                
        });
        return new Mapper(config).Map<AddPatientDTO, Patient>(patient);
    }
    
    public static Patient AutoMapPatientDTO(PatientDTO patient, Guid? genderId)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<PatientDTO, Patient>()
                .ForMember(dist => dist.Gender, opt => opt.Ignore())
                .ForMember(dist => dist.Family, opt => opt.MapFrom(src => src.Name.Family))
                .ForMember(dist => dist.Use, opt => opt.MapFrom(a => a.Name.Use))
                .ForMember(dist => dist.GenderId, opt => opt.MapFrom(a => genderId))
                .ForMember(dist => dist.Id, opt => opt.MapFrom(a => a.Name.Id));
                
        });
        return new Mapper(config).Map<PatientDTO, Patient>(patient);
    }
}