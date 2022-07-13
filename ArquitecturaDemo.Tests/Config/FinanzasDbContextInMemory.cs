namespace SistemaILP.Test.LibroIVA.Config
{
    public class UsersDbContextInMemory
    {
        public static UsersContext Get(bool vacio)
        {
            var InMemoryConnectionString = "DataSource=:memory:";
            var connection = new SqliteConnection(InMemoryConnectionString);
            connection.Open();

            var options = new DbContextOptionsBuilder<UsersContext>()
                .UseSqlite(connection)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var context = new UsersContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!vacio)
            {
                context.AddRange(
                new Usuario()
                {
                    Codigo = "1234",
                    Password = "adfñl",
                    CorreoElectronico = "kristo@gmail.com",
                    Nombre = "Kristo",
                    Apellido = "Calel"
                },
                new Usuario()
                {
                    Codigo = "12345",
                    Password = "adfñl",
                    CorreoElectronico = "silver@gmail.com",
                    Nombre = "Silver",
                    Apellido = "Calel"
                },
                new Usuario()
                {
                    Codigo = "123456",
                    Password = "adfñl",
                    CorreoElectronico = "rodolfo@gmail.com",
                    Nombre = "Rodolfo",
                    Apellido = "Calel"
                });
                context.SaveChanges();
            }

            return context;
        }
    }
}