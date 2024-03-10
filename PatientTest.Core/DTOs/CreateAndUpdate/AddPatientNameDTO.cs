namespace PatientTest.Core.DTOs;

public class AddPatientNameDTO
{
    public string? Use { get; set; }
    public string Family { get; set; }
    public List<string>? Given { get; set; }
}