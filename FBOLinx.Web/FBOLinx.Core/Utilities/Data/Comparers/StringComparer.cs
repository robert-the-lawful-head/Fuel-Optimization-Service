using FBOLinx.Core.Utilities.Data.Cleaners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace FBOLinx.Core.Utilities.Data.Comparers
{
    public class StringComparer
    {
        #region Static Methods
        public static bool AreStringsEquivalent(string string1, string string2)
        {
            return (StringCleaner.CleanInvalidCharacters(string1).ToLower().Trim() ==
                    StringCleaner.CleanInvalidCharacters(string2).ToLower().Trim());
        }

        public static bool IsCurrency(string input)
        {
            Regex ex = new Regex(@"\p{Sc}");
            return !string.IsNullOrEmpty(ex.Match(input).Value);
        }

        public static bool IsAllUpperCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsLetter(input[i]))
                    continue;
                if (!Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }

        public static bool IsValidEmailAddress(string address)
        {
            return (address != null && new EmailAddressAttribute().IsValid(address));

        }

        public static bool IsNumeric(string expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool IsDate(string expression)
        {
            bool isDate;
            DateTime returnDate;
            isDate = DateTime.TryParse(Convert.ToString(expression), out returnDate);
            return isDate;
        }
        #endregion
    }
}
