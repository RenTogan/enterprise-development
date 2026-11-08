IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> db = postgres.AddDatabase("carwashdb");

builder.AddProject<Projects.CarWash_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();