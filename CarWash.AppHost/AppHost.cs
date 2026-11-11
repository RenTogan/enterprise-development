IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> postgresPassword = builder.AddParameter("postgres-password", "postgres", secret: true);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume();

IResourceBuilder<PostgresDatabaseResource> db = postgres.AddDatabase("carwashdb");

builder.AddProject<Projects.CarWash_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();