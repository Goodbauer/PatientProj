using PatientTest.Core.DTOs;

namespace PatientTest.Core.Interfaces.Services;

public interface IPatientService
{
    PatientDTO GetById(Guid id);
    Boolean IsExist(Guid id);
    void Create(AddPatientDTO patient);
    void Delete(Guid id);
    void Update(PatientDTO patient);
    List<PatientDTO> ListAll(List<string> dateParams);
    bool ValidateDateParams(List<string> dateParams);
}