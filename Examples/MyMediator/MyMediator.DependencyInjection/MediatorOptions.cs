using System.Reflection;

namespace MyMediator.DependencyInjection;

public sealed class MediatorOptions
{
    private readonly List<Assembly> _assemblies = [];
    private readonly List<Type> _openBehaviors = [];

    internal IReadOnlyCollection<Assembly> Assemblies => _assemblies;
    internal IReadOnlyCollection<Type> OpenBehaviors => _openBehaviors;

    public MediatorOptions RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        _assemblies.AddRange(assemblies);
        return this;
    }

    public MediatorOptions AddOpenBehavior(Type openBehavior)
    {
        _openBehaviors.Add(openBehavior);
        return this;
    }
}