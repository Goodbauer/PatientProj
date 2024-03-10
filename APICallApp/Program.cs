using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using PatientTest.Core.DTOs;

List<string> names = new List<string>() { "Oliver","Jake","Noah","James","Jack","Connor","Liam","John","Harry","Callum","Mason","Robert","Jacob","Michael","Charlie","Kyle","William","Thomas","Joe","Ethan","David","George","Reece","Michael","Richard","Oscar","Rhys","Alexander","Joseph","James","Charlie","James","Charles","William","Damian","Daniel","Thomas" };

using (var client = new HttpClient())
{
    client.BaseAddress = new Uri("https://localhost:7148");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    Console.WriteLine("Creating");
    
    for (int i = 0; i < 100; i++)
    {
        CreateAsync(client, names).Wait();
    }
    
    Console.WriteLine("Created 100 records");
    //Console.WriteLine("Print all items");
    //GetAsync(client).Wait();
}

static async Task CreateAsync(HttpClient client, List<string> names)
{
    AddPatientDTO newPatient = new AddPatientDTO()
    {
        Active = getRndAction(),
        Gender = getRndGender(),
        BirthDate = RandomDayFunc(),
        Name = new AddPatientNameDTO()
        {
            Family = getRndFamily(names),
            Given = getRndGivens(names),
            Use = getRndUse()
        }
        
    };
    Console.WriteLine(newPatient.Name.Family + " " + newPatient.Gender);

    HttpResponseMessage response = await client.PostAsJsonAsync("api/Patient", newPatient);
    if(response.IsSuccessStatusCode)
    {
       Uri siteUrl = response.Headers.Location;
       Console.WriteLine(siteUrl);
    }
    
}

static string? getRndUse()
{
    Random rnd = new Random();
    int num = rnd.Next(0, 4);
    string? valToReturn = string.Empty;
    switch (num)
    {
        case 1:
            valToReturn = "first option";
            break;
        case 2:
            valToReturn = "second option";
            break;
        case 3:
            valToReturn = "third option";
            break;
        case 4:
            valToReturn = "four option";
            break;
        default:
            valToReturn = null;
            break;
    }

    return valToReturn;
}

static List<string>? getRndGivens(List<string> names)
{
    Random rnd = new Random();
    int num = rnd.Next(0, 3);
    if (num == 0)
        return null;
    List<string> givensToReturn = new List<string>(num);
    for (int i = 0; i < num; i++)
    {
        givensToReturn.Add(names[rnd.Next(0, names.Count - 1)]);
    }

    return givensToReturn;
}

static string getRndFamily(List<string> names)
{
    Random rnd = new Random();
    int num = rnd.Next(0, names.Count - 1);
    return names[num];
}

static bool? getRndAction()
{
    Random rnd = new Random();
    int num = rnd.Next(1, 3);
    return num == 1 ? true : num == 2 ? false : null;
}

static DateTime RandomDayFunc()
{
    DateTime start = new DateTime(1995, 1, 1); 
    Random gen = new Random(); 
    int range = ((TimeSpan)(DateTime.Today - start)).Days; 
    return start.AddDays(gen.Next(range));
}

static string? getRndGender()
{
    Random rnd = new Random();
    int num = rnd.Next(0, 4);

    string? valToReturn = string.Empty;
    switch (num)
    {
        case 1:
            valToReturn = "male";
            break;
        case 2:
            valToReturn = "female";
            break;
        case 3:
            valToReturn = "other";
            break;
        case 4:
            valToReturn = "unknown";
            break;
        default:
            valToReturn = null;
            break;
    }

    return valToReturn;
}

static async Task GetAsync(HttpClient client)
{
        Console.WriteLine("Get all");
        HttpResponseMessage response = await client.GetAsync("/api/Patient");
        if(response.IsSuccessStatusCode)
        {
            List<PatientDTO>? patients = await response.Content.ReadFromJsonAsync<List<PatientDTO>>();
            foreach (var patient in patients)
            {
                Console.WriteLine("Name: " + patient.Name.Family);
            }
            
        }
}