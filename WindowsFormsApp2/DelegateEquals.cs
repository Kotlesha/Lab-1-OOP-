using System;
using System.Linq;
using System.Reflection;

namespace WindowsFormsApp2
{
    public static class DelegateEquals
    {
        public static bool EqualsMethods(this Func<decimal, bool> delegate1, Func<decimal, bool> delegate2)
        {
            if (delegate1 is null)
            {
                return false;
            }

            if (delegate2 is null)
            {
                return false;
            }

            MethodBody m1 = delegate1.Method.GetMethodBody();
            MethodBody m2 = delegate2.Method.GetMethodBody();
            byte[] il1 = m1.GetILAsByteArray();
            byte[] il2 = m2.GetILAsByteArray();
            return il1.SequenceEqual(il2);
        }
    }
}
