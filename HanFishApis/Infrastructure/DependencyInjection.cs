using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using System.Runtime.Loader;

namespace HanFishApis.Infrastructure
{
    public static class DependencyInjection
    {
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        public static void AddAssembly(this IServiceCollection services, string assemblyName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrEmpty(assemblyName)) throw new ArgumentNullException(nameof(assemblyName));

            var assembly = GetAssemblyByName(assemblyName);
            if (assembly == null) throw new DllNotFoundException(nameof(assembly));

            var list = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract).ToList();
            foreach (var type in list)
            {
                var interfacesList = type.GetInterfaces();
                if (!interfacesList.Any())
                    continue;

                var innerLifetime = serviceLifetime;
                foreach (Type serviceType in interfacesList)
                {
                    switch (innerLifetime)
                    {
                        case ServiceLifetime.Singleton:
                            services.TryAddSingleton(serviceType, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.TryAddScoped(serviceType, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.TryAddTransient(serviceType, type);
                            break;
                    }
                }
            }
        }

    }
}
