﻿namespace MiniExcelLibs.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal static class Helpers
    {
	   private static readonly Regex EscapeRegex = new Regex("_x([0-9A-F]{4,4})_");

	   public static IDictionary<string, object> GetEmptyExpandoObject(int maxColumnIndex)
	   {
		  // TODO: strong type mapping can ignore this
		  // TODO: it can recode better performance 
		  var cell = (IDictionary<string, object>)new ExpandoObject();
		  for (int i = 0; i <= maxColumnIndex; i++)
			 cell.Add(i.ToString(), null);
		  return cell;
	   }

	   public static IDictionary<string, object> GetEmptyExpandoObject(Dictionary<int, string> hearrows)
	   {
		  // TODO: strong type mapping can ignore this
		  // TODO: it can recode better performance 
		  var cell = (IDictionary<string, object>)new ExpandoObject();
		  foreach (var hr in hearrows)
			 cell.Add(hr.Value, null);
		  return cell;
	   }


	   public static string ConvertEscapeChars(string input)
	   {
		  return EscapeRegex.Replace(input, m => ((char)uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString());
	   }

	   /// <summary>
	   /// Convert a double from Excel to an OA DateTime double. 
	   /// The returned value is normalized to the '1900' date mode and adjusted for the 1900 leap year bug.
	   /// </summary>
	   public static double AdjustOADateTime(double value, bool date1904)
	   {
		  if (!date1904)
		  {
			 // Workaround for 1900 leap year bug in Excel
			 if (value >= 0.0 && value < 60.0)
				return value + 1;
		  }
		  else
		  {
			 return value + 1462.0;
		  }

		  return value;
	   }

	   public static bool IsValidOADateTime(double value)
	   {
		  return value > DateTimeHelper.OADateMinAsDouble && value < DateTimeHelper.OADateMaxAsDouble;
	   }

	   public static object ConvertFromOATime(double value, bool date1904)
	   {
		  var dateValue = AdjustOADateTime(value, date1904);
		  if (IsValidOADateTime(dateValue))
			 return DateTimeHelper.FromOADate(dateValue);
		  return value;
	   }
    }

}
