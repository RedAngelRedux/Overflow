# Adding a Microservice to Aspire AppHost

## Overview
This document outlines the steps to add a new microservice (e.g., QuestionService) to an Aspire-based distributed application and configure it to work with external dependencies like Keycloak.

## Changes Required

### 1. AppHost.cs - Refactor to Class-Based Structure
Convert from top-level statements to a proper class structure:

```csharp
namespace Overflow.AppHost;

using Projects;

public class AppHost
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        // Existing infrastructure services
        var keycloak = builder.AddKeycloak("keycloak", 6001)
            .WithDataVolume("keycloak-data");

        // New microservice with dependencies
        var questionService = builder.AddProject<QuestionService>("question-service")
            .WithReference(keycloak)
            .WaitFor(keycloak);

        builder.Build().Run();
    }
}
```

**Key Pattern:**
- `AddProject<T>()` - Registers the service with type-safe reference
- `WithReference()` - Makes the service aware of its dependencies (exposes as environment variables)
- `WaitFor()` - Ensures startup order; service won't start until dependency is ready

### 2. Overflow.AppHost.csproj - Add Project Reference
```xml
<ItemGroup>
    <ProjectReference Include="..\QuestionService\QuestionService.csproj" />
</ItemGroup>
```

### 3. Overflow.slnx - Organize Projects
Create a `/services/` folder to group microservices separately from infrastructure:

```xml
<Solution>
  <Folder Name="/services/">
    <Project Path="QuestionService/QuestionService.csproj" Id="[GUID]" />
  </Folder>
  <Project Path="Overflow.AppHost/Overflow.AppHost.csproj" />
  <Project Path="Overflow.ServiceDefaults/Overflow.ServiceDefaults.csproj" />
</Solution>
```

## For Future Services

When adding the next microservice (e.g., AnswerService):

1. Create the new project folder and `.csproj`
2. Add `<ProjectReference>` to `Overflow.AppHost.csproj`
3. Add project entry to `/services/` folder in `.slnx`
4. In `AppHost.Main()`:
```csharp
   var answerService = builder.AddProject<AnswerService>("answer-service")
       .WithReference(keycloak)
       .WithReference(questionService)  // If it depends on QuestionService
       .WaitFor(keycloak)
       .WaitFor(questionService);
```

## Notes
- Service names (e.g., "question-service") should be kebab-case and match the resource name for discovery
- Dependencies must be registered before dependent services in AppHost.Main()
- `WithReference()` automatically handles environment variable injection for service discovery
