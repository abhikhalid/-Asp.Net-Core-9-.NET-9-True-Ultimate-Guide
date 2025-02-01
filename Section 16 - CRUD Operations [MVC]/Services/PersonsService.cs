using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
               _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("F594D20E-8CF8-4887-8CBF-8CC0925F4A9A"),
                    PersonName = "Jareb",
                    Email = "jjorgensen0@wisc.edu",
                    DateOfBirth = DateTime.Parse("12/8/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = true,
                    Address = "Dhaka",
                    CountryID = Guid.Parse("9C0CAB52-4CA3-4F38-A2B8-1255D8BFD232")
                });
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("4FC86808-AC69-4F14-8002-465665D3EC02"),
                    PersonName = "Currie",
                    Email = "cupstell1@ibm.com",
                    DateOfBirth = DateTime.Parse("3/22/2024"),
                    Gender = "Female",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("9C0CAB52-4CA3-4F38-A2B8-1255D8BFD232")
                }); 
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("7C5540CE-CFE8-44F9-98EC-D1706B9C07FB"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("9C0CAB52-4CA3-4F38-A2B8-1255D8BFD232")
                });
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("BAFADA56-1069-4FB6-B58F-C8586D074078"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("9C0CAB52-4CA3-4F38-A2B8-1255D8BFD232")
                }); 
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("71064E7E-F41F-4AEF-B342-4F87460C6B7B"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("D7E9512D-2845-4728-8E54-1AA478141FF4")
                });
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("27BFA180-EF71-4DC1-B120-EA61DABDACB4"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("D7E9512D-2845-4728-8E54-1AA478141FF4")
                }); 
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("A45B55E9-EC6F-4548-A4B8-ECD382405FB4"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("526922C9-4B81-42D6-A417-8A2D2F5F6482")
                });
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("76CA5315-9697-4E16-B93E-2AE65F529D3C"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("9D7ECC1B-F7AD-4754-9951-F303D9DE83A7")
                }); 
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("E5009D2C-E94B-4A1E-940B-EF69A51C1FF6"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("9D7ECC1B-F7AD-4754-9951-F303D9DE83A7")
                });
                
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("32BD53DB-5DE7-44E2-A918-6C0E0F5A5431"),
                    PersonName = "Jorgan",
                    Email = "jgoade2@sciencedaily.com",
                    DateOfBirth = DateTime.Parse("10/28/2024"),
                    Gender = "Male",
                    ReciveNewsLetters = false,
                    CountryID = Guid.Parse("554C00E8-1114-4F3E-99C8-8D895DACE4E1")
                });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(person);

            //Convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
           return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if(person == null) return null;

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return matchingPersons;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ?
                        temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break; 
                
                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ?
                        temp.Gender.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                //if sortBy = PersonName and sortOrder = Ascending Order
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                    => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                    => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                   => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                  => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                 => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson =  _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

            if(matchingPerson == null)
            {
                throw new ArgumentException("Given Person Id does not exist");
            }
            
            //update all deatails
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReciveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null) throw new ArgumentNullException(nameof(personID));

            Person? person =  _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if (person == null) return false;

            _persons.RemoveAll(temp => temp.PersonID == personID);
            
            return true;
        }
    }
}
