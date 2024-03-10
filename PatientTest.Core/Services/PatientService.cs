using System.Linq.Expressions;
using PatientTest.Core.DTOs;
using PatientTest.Core.Entities;
using PatientTest.Core.Enum;
using PatientTest.Core.Interfaces.Generic;
using PatientTest.Core.Interfaces.Services;
using PatientTest.Core.Utilits;

namespace PatientTest.Core.Services;

public class PatientService : IPatientService
{
    private readonly IRepository<Patient> _patientRepository;
    private readonly IRepository<Given> _givenRepository;
    private readonly IRepository<Genders> _gendersRepository;

    public PatientService(IRepository<Patient> patientRepository, IRepository<Given> givenRepository, IRepository<Genders> gendersRepository)
    {
        _patientRepository = patientRepository;
        _givenRepository = givenRepository;
        _gendersRepository = gendersRepository;
    }
    

    public PatientDTO GetById(Guid id)
    {   
        return AutoMapperDTO.AutoMapPatient(
            _patientRepository.GetResultSpec(p => p.Where(a => a.Id == id)), 
            _givenRepository.GetListResultSpec(g =>g.Where(t => t.PatientId == id)).Select(a =>a.Name).ToList());
        
    }

    public Boolean IsExist(Guid id)
    {
        return _patientRepository.GetResultSpec(p => p.Any(i => i.Id == id));
    }

    public void Create(AddPatientDTO patient)
    {
        Guid? genId = null;
        if(patient.Gender is not null)
            genId = _gendersRepository.GetResultSpec(q => q.Where(a => a.Name.ToLower() == patient.Gender.ToLower()).FirstOrDefault())?.Id;
        
        var patientId = _patientRepository.Create(AutoMapperDTO.AutoMapPatientDTO(patient, genId));
        if (patient.Name.Given is not null)
        {
            _givenRepository.CreateRange(patient.Name.Given.Select(e => new Given()
            {
                PatientId = patientId,
                Name = e
            }));
        }
    }

    public void Delete(Guid id)
    {
        _givenRepository.DeleteRangeByPredicate(s => s.PatientId == id);
        _patientRepository.Delete(id);
    }

    public void Update(PatientDTO patient)
    {
        Boolean val = false;
        var savedGivens = _givenRepository.GetListResultSpec(q => q.Where(a => a.PatientId == patient.Name.Id)).ToList();
        if (patient.Name.Given.All(savedGivens.Select(c => c.Name).Contains) is not true)
        {
            UpdateGivens(savedGivens, patient.Name.Given, patient.Name.Id);
        }
        
        var genId = _gendersRepository.GetResultSpec(q => q.Where(a => a.Name.ToLower() == patient.Gender.ToLower())).FirstOrDefault()?.Id;
        var valToUpd = AutoMapperDTO.AutoMapPatientDTO(patient, genId);
        _patientRepository.Update(valToUpd);
    }

    private void UpdateGivens(List<Given> storedGivens, List<string> newGivens, Guid patientId)
    {
        var itemsToDelete = storedGivens.Where(s => !newGivens.Contains(s.Name)).ToList();
        var itemsToCreate = newGivens.Except(storedGivens.Select(c => c.Name));
        _givenRepository.DeleteRange(itemsToDelete);
        _givenRepository.CreateRange(itemsToCreate.Select(e => new Given()
        {
            PatientId = patientId,
            Name = e
        }));
    }

    public bool ValidateDateParams(List<string> dateParams)
    {
        return dateParams.ToList().All(x => System.Enum.IsDefined(typeof(PossibleDateParams), x.Substring(0,2)));
    }

    public List<PatientDTO> ListAll(List<string> dateParams)
    {
        var predicate = CreatePredicate(dateParams);
        var listPatients = _patientRepository.GetListResultSpec(p => p.Where(predicate));
        var distIdsPatients = listPatients.Select(a => a.Id).ToList();
        var givens = _givenRepository.GetListResultSpec(g => g.Where(a => distIdsPatients.Contains(a.PatientId)))
            .ToList();
        List<PatientDTO> returnedItems = new List<PatientDTO>(distIdsPatients.Count);
        foreach (var patient in listPatients)
        {
            returnedItems.Add(AutoMapperDTO.AutoMapListPatient(patient, givens.Where(a => a.PatientId == patient.Id).Select(p => p.Name).ToList()));
        }

        return returnedItems;
    }
    
    private Expression<Func<Patient, bool>> CreatePredicate(List<string> searchModel)
    {
        var predicate = PredicateBuilder.True<Patient>();
        foreach (var itemToSearch in searchModel)
        {
            switch (itemToSearch.Substring(0, 2))
            {
                case nameof(PossibleDateParams.eq):
                    predicate = predicate.And(p => p.BirthDate.Date == DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
                case nameof(PossibleDateParams.ne):
                    predicate = predicate.And(p => p.BirthDate.Date != DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
                case nameof(PossibleDateParams.lt):
                    predicate = predicate.And(p => p.BirthDate < DateTime.Parse(itemToSearch.Substring(2)));
                    break;
                case nameof(PossibleDateParams.gt):
                    predicate = predicate.And(p => p.BirthDate > DateTime.Parse(itemToSearch.Substring(2)));
                    break;
                case nameof(PossibleDateParams.ge):
                    predicate = predicate.And(p => p.BirthDate.Date > DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
                case nameof(PossibleDateParams.le):
                    predicate = predicate.And(p => p.BirthDate.Date < DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
                case nameof(PossibleDateParams.sa):
                    predicate = predicate.And(p => p.BirthDate < DateTime.Parse(itemToSearch.Substring(2)));
                    break;
                case nameof(PossibleDateParams.eb):
                    predicate = predicate.And(p => p.BirthDate.Date < DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
                case nameof(PossibleDateParams.ap):
                    predicate = predicate.And(p => p.BirthDate.Date == DateTime.Parse(itemToSearch.Substring(2)).Date);
                    break;
            }
        }


        return predicate;
    }
}