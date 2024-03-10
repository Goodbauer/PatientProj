namespace PatientTest.Core.Entities;

public class Patient: Base
{
    public string? Use { get; set; }
    public string Family { get; set; } = string.Empty;
    public Guid? GenderId { get; set; }
    public DateTime BirthDate { get; set; }
    public Boolean? Active { get; set; }
    public Genders? Gender { get; set; }
}