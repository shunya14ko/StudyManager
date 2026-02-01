var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.StudyManager_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.TaskManager>("TaskManager").WithReference(apiService);

builder.Build().Run();
