namespace PatientTest.Core.Interfaces.Generic;

public interface ICRUD<C, R, U> where C : class where R : class where U : class
{
    void Create(C entityDTO);
    void Update(U entityDTO);
    R GetById(int id);
    void Delete(int id);
    bool IsExists(int id);
}