namespace Vortexia.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Vortexia.Core.Events;

    /// <summary>
    /// Extension methods for configuring Vortexia Core services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Vortexia Core services to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddVortexiaCore(this IServiceCollection services)
        {
            // Register the event bus
            services.AddSingleton<IEventBus, EventBus>();

            // Register the Master Controller
            services.AddSingleton<IMasterController, MasterController>();

            return services;
        }
    }
}
