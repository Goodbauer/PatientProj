using Microsoft.AspNetCore.Mvc;
using PatientTest.Core.DTOs;
using PatientTest.Core.Interfaces.Services;

namespace PatientTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public ActionResult GetAll([FromQuery]string[] datesFilters)
    {
        if (!_patientService.ValidateDateParams(datesFilters.ToList()))
            return BadRequest("Date params is not valid");
        
        return Ok(_patientService.ListAll(datesFilters.ToList()));
    }
    
    [HttpGet("{id}")]
    public ActionResult GetById(Guid id)
    {
        if (!_patientService.IsExist(id))
            return NotFound();
        
        return Ok(_patientService.GetById(id));
    }
    
    [HttpPost]
    public ActionResult CreatePatient(AddPatientDTO patient)
    {
        _patientService.Create(patient);
        return Ok();
    }
    
    [HttpPut]
    public ActionResult UpdatePatient(PatientDTO patient)
    {
        if (!_patientService.IsExist(patient.Name.Id))
            return NotFound();
        _patientService.Update(patient);
        
        return Ok();
    }
    
    [HttpDelete]
    public ActionResult DeletePatient(Guid id)
    {
        _patientService.Delete(id);
        return Ok();
    }
}