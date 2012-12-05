
using System;
using System.Reflection;

namespace QuickUnit
{
    internal static class ReflectionMethodExtensions
    {
        public static object InvokeNonPublicX(this object source, string methodName, params object[] arguments)
        {
            try
            {
                MethodInfo method = source.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1 &&
                    (parameters[0].ParameterType.IsArray || arguments == null))
                {
                    return method.Invoke(source, new object[] { arguments });
                }
                return method.Invoke(source, arguments);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
        public static object InvokeStaticNonPublicX(this Type source, string methodName, params object[] arguments)
        {
            try
            {
                MethodInfo method = source.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic| BindingFlags.Public);
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1 && 
                    (parameters[0].ParameterType.IsArray || arguments == null))
                {
                    return method.Invoke(null, new object[]{arguments});
                }
                return method.Invoke(null, arguments);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}