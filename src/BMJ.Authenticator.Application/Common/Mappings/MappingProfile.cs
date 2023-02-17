using AutoMapper;
using System.Reflection;

namespace BMJ.Authenticator.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);

        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            ExecuteMapping(
                type.ContainsGenericParameters 
                    ? type.MakeGenericType(new Type[] { typeof(object) })
                    : type, 
                mappingMethodName, 
                HasInterface, 
                argumentTypes);
        }
    }

    private void ExecuteMapping(
        Type type,
        string mappingMethodName,
        Func<Type, bool> HasInterface,
        Type[] argumentTypes)
    {
        var instance = Activator.CreateInstance(type);

        var methodInfo = type.GetMethod(mappingMethodName);

        if (methodInfo != null)
        {
            methodInfo.Invoke(instance, new object[] { this });
        }
        else
        {
            var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

            if (interfaces.Count > 0)
            {
                foreach (var @interface in interfaces)
                {
                    var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                    interfaceMethodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}
