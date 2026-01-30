namespace Overflow.AppHost;

using Projects;

public class AppHost
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var keycloak = builder.AddKeycloak("keycloak", 6001)
            .WithDataVolume("keycloak-data");

        var questionService = builder.AddProject<QuestionService>("question-service")
            .WithReference(keycloak)
            .WaitFor(keycloak);

        builder.Build().Run();
    }
}
