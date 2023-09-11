using Bogus;
using Bogus.DataSets;

namespace TestDataGenerator
{
    public class Program
    {
        public static void Main()
        {
            var ageCertifications = new List<string> { "G", "PG", "PG-13", "R", "NC-17", "U", "U/A", "A", "S", "AL", "6", "9", "12", "12A", "15", "18", "18R", "R18", "R21", "M", "MA15+", "R16", "R18+", "X18", "T", "E", "E10+", "EC", "C", "CA", "GP", "M/PG", "TV-Y", "TV-Y7", "TV-G", "TV-PG", "TV-14", "TV-MA" };
            var roles = new List<string> { "Director", "Producer", "Screenwriter", "Actor", "Actress", "Cinematographer", "Film Editor", "Production Designer", "Costume Designer", "Music Composer" };
            var genres = new List<string> { "Action", "Comedy", "Drama", "Horror", "Sci-Fi", "Romance", "Thriller", "Adventure", "Mystery", "Fantasy" };

            var titlesFaker = new Faker<Titles>()
                .RuleFor(t => t.Id, f => f.IndexFaker + 1)
                .RuleFor(t => t.Title, f => f.Lorem.Sentence())
                .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                .RuleFor(t => t.ReleaseYear, f => f.Date.Between(new DateTime(1900, 1, 1), DateTime.Now).Year)
                .RuleFor(t => t.AgeCertification, f => f.PickRandom(ageCertifications))
                .RuleFor(t => t.Runtime, f => f.Random.Int(30, 300))
                .RuleFor(t => t.Genres, f => string.Join(";", f.PickRandom(genres, f.Random.Int(1, genres.Count))))
                .RuleFor(t => t.ProductionCountry, f => f.Address.CountryCode(Iso3166Format.Alpha3))
                .RuleFor(t => t.Seasons, f => f.Random.Bool(0.25f) ? (int?)f.Random.Int(1, 10) : null);

            var titlesList = titlesFaker.Generate(150);

            using (var writer = new StreamWriter("titles.csv"))
            {
                writer.WriteLine("Id,Title,Description,ReleaseYear,AgeCertification,Runtime,Genres,ProductionCountry,Seasons");
                foreach (var title in titlesList)
                {
                    writer.WriteLine($"{title.Id},{title.Title},{title.Description},{title.ReleaseYear},{title.AgeCertification},{title.Runtime},{title.Genres},{title.ProductionCountry},{title.Seasons}");
                }
            }

            var creditsFaker = new Faker<Credits>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.TitleId, f => f.PickRandom(titlesList).Id)
                .RuleFor(c => c.RealName, f => f.Person.FullName)
                .RuleFor(c => c.CharacterName, f => f.Name.FullName())
                .RuleFor(c => c.Role, f => f.PickRandom(roles));

            var creditsList = creditsFaker.Generate(150);

            using (var writer = new StreamWriter("credits.csv"))
            {
                writer.WriteLine("Id,TitleId,RealName,CharacterName,Role");
                foreach (var credit in creditsList)
                {
                    writer.WriteLine($"{credit.Id},{credit.TitleId},{credit.RealName},{credit.CharacterName},{credit.Role}");
                }
            }
        }
    }
}