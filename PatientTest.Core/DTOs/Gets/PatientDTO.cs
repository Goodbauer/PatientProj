namespace PatientTest.Core.DTOs;

public class PatientDTO
{
    public PatientNameDTO? Name { get; set; }
    public string? Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public Boolean? Active { get; set; }
}