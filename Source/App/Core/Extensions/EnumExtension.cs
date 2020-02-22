using System;
using System.ComponentModel;
using System.Reflection;

namespace Project.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum en) //ext method

        {

            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)

            {

                object[] attrs = memInfo[0].GetCustomAttributes(
                    typeof(DescriptionAttribute),

                    false);

                if (attrs.Length > 0)

                    return ((DescriptionAttribute)attrs[0]).Description;

            }

            return en.ToString();

        }
    }
}
