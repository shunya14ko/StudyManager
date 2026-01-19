var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.StudyManager_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.StudyManager_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.TimeManager>("timemanager").WithReference(apiService);

builder.Build().Run();
