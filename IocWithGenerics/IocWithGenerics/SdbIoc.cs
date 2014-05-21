using System;
using System.Collections.Generic;


namespace IocWithGenerics
{
    public class SdbIoc
    {
        private Dictionary<Type,Type> _registry = new Dictionary<Type, Type>();

        public void Register<TInt, TClass>() where TClass:TInt
        {
            _registry.Add(typeof(TInt),typeof(TClass));
        }

        public T Resolve<T>()
        {
            var resolvedType = _registry[typeof(T)];

            var classToReturn = (T)Activator.CreateInstance(resolvedType);

            return classToReturn;
        }
    }
}
