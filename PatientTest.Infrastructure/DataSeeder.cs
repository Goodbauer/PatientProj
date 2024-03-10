using PatientTest.Core.Entities;

namespace PatientTest.Infrastructure;

public class DataSeeder
{
    private static Context _context;
    public static void SeedData(Context context)
    {
        _context = context;
        AddGenders();
    }

    public static void AddGenders()
    {
        if (_context.Genders.Count() < 1)
        {
            List<Genders> gendersList = new List<Genders>()
            {
                new Genders
                {
                    Name = "Male"
                },
                new Genders
                {
                    Name = "Female"
                },
                new Genders
                {
                    Name = "Other"
                },
                new Genders
                {
                    Name = "Unknown"
                }
            };
            
            _context.Genders.AddRange(gendersList);
            _context.SaveChanges();

        }
    }
}