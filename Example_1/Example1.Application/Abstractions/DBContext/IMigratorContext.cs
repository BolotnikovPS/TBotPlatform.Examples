namespace Example1.Application.Abstractions.DBContext;

public interface IMigratorContext
{
    Task MigrateAsync();
}