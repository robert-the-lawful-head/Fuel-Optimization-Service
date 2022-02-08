using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace FBOLinx.Core.Utilities.Enums
{
    public class EnumHelper
    {
        public static string GetDescription(System.Enum value)
        {
            if (value == null)
                return "";
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();
            }
            catch (System.Exception exception)
            {
                return "";
            }
        }

        public static IEnumerable<EnumDescriptionValue> GetDescriptions(Type type)
        {
            var descriptions = new List<EnumDescriptionValue>();
            var values = System.Enum.GetValues(type);
            foreach (object value in values)
            {
                var field = type.GetField(value.ToString());
                var description = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                descriptions.Add(new EnumDescriptionValue()
                {
                    Description = ((DescriptionAttribute)description[0]).Description,
                    Value = System.Convert.ToInt16(value)
                });
            }
            return descriptions;
        }

        public static Type GetEnumTypeByName(string enumName)
        {
            try
            {
                if (!enumName.Contains("."))
                    enumName = "Fuelerlinx.Core.Enums." + enumName;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var type = assembly.GetType(enumName);
                    if (type == null)
                        continue;
                    if (type.IsEnum)
                        return type;
                }
            }
            catch (System.Exception exception)
            {

            }

            return null;
        }

        #region Objects
        public class EnumDescriptionValue
        {
            public string Description { get; set; }
            public short Value { get; set; }
        }
        #endregion
    }
}
