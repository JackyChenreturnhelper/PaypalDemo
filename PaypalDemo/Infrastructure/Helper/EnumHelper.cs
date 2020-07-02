using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.Helper
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, (object)value);
            if (name == null)
                return string.Empty;
            FieldInfo field = type.GetField(name);
            return field == (FieldInfo)null ? name : (Attribute.GetCustomAttribute((MemberInfo)field, typeof(DescriptionAttribute)) is DescriptionAttribute customAttribute ? customAttribute.Description : (string)null) ?? name;
        }

        public static bool TryParseByNameOrDescription<TEnum>(this string str, out TEnum result) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidOperationException("T is not enum type");
            return Enum.TryParse<TEnum>(str, out result) || str.TryParseByDescription<TEnum>(out result);
        }

        public static bool TryParseByDescription<TEnum>(this string description, out TEnum result)
        {
            Type type = typeof(TEnum);
            if (!type.IsEnum)
                throw new InvalidOperationException("T is not enum type");
            bool flag = false;
            result = default(TEnum);
            foreach (FieldInfo field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute((MemberInfo)field, typeof(DescriptionAttribute)) is DescriptionAttribute customAttribute && customAttribute.Description == description)
                {
                    flag = true;
                    result = (TEnum)field.GetValue((object)null);
                    break;
                }
            }
            return flag;
        }

        public static List<TEnum> ToListEnumByNameOrDescription<TEnum>(this string jsonStr) where TEnum : struct, Enum
        {
            List<TEnum> enumList = new List<TEnum>();
            foreach (string str in JsonConvert.DeserializeObject<List<string>>(jsonStr))
            {
                TEnum result;
                if (str.TryParseByNameOrDescription<TEnum>(out result))
                    enumList.Add(result);
            }
            return enumList;
        }
    }
}