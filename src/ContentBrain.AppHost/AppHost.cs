using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentBrain.AppHost
{
    // Small local implementation of a DistributedApplication builder used by the AppHost
    // This mirrors the intent of Aspire composition and provides a simple fluent API
    // to register composed services and service references for local development.
    public sealed class DistributedApplication
    {
        public record Service(string Name, string ProjectOrImage, int Port, bool Expose);
        public record ServiceReference(string Name, string Url);

        private readonly List<Service> _services = new();
        private readonly List<ServiceReference> _serviceReferences = new();

        private DistributedApplication() { }

        public IReadOnlyCollection<Service> Services => _services.AsReadOnly();
        public IReadOnlyCollection<ServiceReference> ServiceReferences => _serviceReferences.AsReadOnly();

        public int Run()
        {
            Console.WriteLine("ContentBrain AppHost composition:\n");

            Console.WriteLine("Services:");
            foreach (var s in _services)
            {
                Console.WriteLine($" - {s.Name}: {s.ProjectOrImage} (port={s.Port}, expose={s.Expose})");
            }

            Console.WriteLine();
            Console.WriteLine("Service references:");
            foreach (var r in _serviceReferences)
            {
                Console.WriteLine($" - {r.Name} => {r.Url}");
            }

            Console.WriteLine();
            Console.WriteLine("This AppHost mirrors the previous aspire.yaml composition. Use Aspire tooling or the project references to run individual services during development.");
            return 0;
        }

        // Builder implementation
        public sealed class Builder
        {
            private readonly DistributedApplication _app = new();
            private readonly string[] _args;

            internal Builder(string[] args) => _args = args ?? Array.Empty<string>();

            public Builder AddProjectService(string name, string projectPath, int port, bool expose = true)
            {
                _app._services.Add(new Service(name, projectPath, port, expose));
                return this;
            }

            public Builder AddContainerService(string name, string image, int port, bool expose = false)
            {
                _app._services.Add(new Service(name, image, port, expose));
                return this;
            }

            public Builder AddServiceReference(string name, string url)
            {
                _app._serviceReferences.Add(new ServiceReference(name, url));
                return this;
            }

            public DistributedApplication Build() => _app;
        }

        public static Builder CreateBuilder(string[] args) => new Builder(args);
    }

    public static class AppHost
    {
        // Entry point for the AppHost using the DistributedApplication builder pattern.
        public static int Main(string[] args)
        {
            var app = DistributedApplication.CreateBuilder(args)
                .AddProjectService("web", "../ContentBrain.Web/ContentBrain.Web.csproj", 5002, expose: true)
                .AddProjectService("api", "../ContentBrain.Api/ContentBrain.Api.csproj", 5001, expose: true)
                .AddContainerService("postgres", "postgres:15", 5432, expose: false)
                .AddServiceReference("Api", "http://api:5001/")
                .AddServiceReference("Web", "http://web:5002/")
                .Build();

            return app.Run();
        }
    }
}
