using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace FBOLinx.Core.Utilities
{
    public class Enum
    {
        public static string GetDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static IEnumerable<EnumDescriptionValue> GetDescriptions(Type type)
        {
            var descriptions = new List<EnumDescriptionValue>();
            var values = System.Enum.GetValues(type);
            foreach (object value in values)
            {
                var field =    type.GetField(value.ToString());
                var description = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                descriptions.Add(new EnumDescriptionValue() {
                    Description = ((DescriptionAttribute) description[0]).Description,
                    Value = value});
            }
            return descriptions;
        }

        #region Objects
        public class EnumDescriptionValue
        {
            public string Description { get; set; }
            public object Value { get; set; }
        }
        #endregion
    }
}
