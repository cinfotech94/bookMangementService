using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BookManagementService.API.Extensions
{


    public static class FluentValidationConfiguration
    {
        public static void AddFluentValidationConfiguration(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Iterate over each provided assembly
            foreach (var assembly in assemblies)
            {
                // Find all types that implement IValidator and are not abstract
                var validators = assembly.GetTypes()
                    .Where(type => typeof(IValidator).IsAssignableFrom(type) && !type.IsAbstract);

                // Register each validator as a transient service
                foreach (var validator in validators)
                {
                    services.AddTransient(typeof(IValidator), validator);
                }
            }
        }
    }

}
