namespace PatientTest.Core.Entities;

public class Given: Base
{
    public string Name { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }
}