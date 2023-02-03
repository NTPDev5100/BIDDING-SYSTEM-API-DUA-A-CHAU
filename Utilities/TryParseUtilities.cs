using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class TryParseUtilities
    {
        public static double? TryParseDouble(this string textString)
        {
            double value = 0;
            if (string.IsNullOrEmpty(textString) || !double.TryParse(textString, out value))
                return null;
            return value;
        }
    }
}
