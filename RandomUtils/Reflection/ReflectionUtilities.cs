using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Reflection
{
    internal static class ReflectionUtilities
    {
        internal static bool HasDefaultConstructor(this Type type)
        {
            try
            {
                int minParameterCount = type.GetConstructors()
                    .Where(constr => constr.GetParameters().Count() == 0).Count();
                if (minParameterCount == 1 || type.IsSerializable)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
