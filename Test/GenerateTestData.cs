using Bogus;
using Domain;

namespace Test.API.TestData
{
    public class TestData
    {
        private readonly Faker _faker;

        public TestData()
        {
            _faker = new Faker();
        }

        public Company CreateCompany()
        {
            var company = new Company
            {
                Email = _faker.Internet.Email(),
                Kvk = _faker.Random.Number(10000000, 99999999).ToString(),
                CompanyName = _faker.Company.CompanyName(),
                Phone = _faker.Phone.PhoneNumber(),
                Address = _faker.Address.StreetAddress(),
                PostalCode = _faker.Address.ZipCode(),
                Province = _faker.Address.State(),
                Country = _faker.Address.Country(),
                WebsiteUrl = _faker.Internet.Url(),
                ContactPerson = _faker.Name.FullName()
            };
            return company;
        }

        public PanelMember CreatePanelMember()
        {
            var panelMember = new PanelMember
            {
                Email = _faker.Internet.Email(),
                Guardian = _faker.Random.Number(1, 1000),
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                DateOfBirth = _faker.Date.Past(30),
                Address = _faker.Address.StreetAddress(),
                PostalCode = _faker.Address.ZipCode(),
                City = _faker.Address.City(),
                Country = _faker.Address.Country()
            };
            return panelMember;
        }

        public Research CreateRandomResearch()
        {
            var research = new Research
            {
                Title = _faker.Lorem.Sentence(),
                Description = _faker.Lorem.Paragraph(),
                Date = _faker.Date.Future(),
                Type = _faker.PickRandom("Online", "Offline"),
                Category = _faker.Lorem.Word(),
                Reward = _faker.Random.Double(1, 1000),
                Organizer = CreateCompany(),
                Participants = new List<ResearchParticipant>(){
                    new ResearchParticipant{
                    PanelMember = CreatePanelMember(),
                    DateJoined = _faker.Date.FutureOffset(30).DateTime
                }
            }
            };
            return research;
        }
    }
}
