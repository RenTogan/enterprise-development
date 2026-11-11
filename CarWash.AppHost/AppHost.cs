var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", "postgres", secret: true);

var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithDataVolume();

var db = postgres.AddDatabase("carwashdb");

builder.AddProject<Projects.CarWash_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();