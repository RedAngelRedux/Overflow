namespace Overflow.AppHost;

using Projects;

public class AppHost
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable ASPIRECERTIFICATES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        var keycloak = builder.AddKeycloak("keycloak", 6001)
            .WithoutHttpsCertificate()  // This is just temporary so we can test using Postman and ensure it is pinging the non-ssl url
            .WithDataVolume("keycloak-data");
#pragma warning restore ASPIRECERTIFICATES001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        var questionService = builder.AddProject<QuestionService>("question-service")
            .WithReference(keycloak)
            .WaitFor(keycloak);

        builder.Build().Run();
    }
}
