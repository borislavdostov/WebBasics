using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicHttpServer.MvcFramework
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type, Type> dependencyContainer = new Dictionary<Type, Type>();

        public void Add<TSource, TDestination>()
        {
            dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            if (dependencyContainer.ContainsKey(type))
            {
                type = dependencyContainer[type];
            }

            var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Count()).FirstOrDefault();
            var parameters = constructor.GetParameters();
            var parameterValues = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterValue = CreateInstance(parameter.ParameterType);
                parameterValues.Add(parameterValue);
            }

            var obj = constructor.Invoke(parameterValues.ToArray());
            return obj;
        }
    }
}
