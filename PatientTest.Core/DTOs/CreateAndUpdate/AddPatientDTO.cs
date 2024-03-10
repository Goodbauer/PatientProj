namespace PatientTest.Core.DTOs;

public class AddPatientDTO
{
    public AddPatientNameDTO Name { get; set; }
    public string? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public Boolean? Active { get; set; }
}