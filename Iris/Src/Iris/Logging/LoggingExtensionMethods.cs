﻿using System;
using System.Globalization;

namespace Iris.Logging
{
    internal static class LoggingExtensionMethods
    {
        private const string MessageFormat = "{0:yyyy/MM/dd HH:mm:ss.ff} - {1} - {2}";

        public static string FormatMessage(this string message, Type typeToLog, params object[] values)
        {
            try
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    MessageFormat,
                    DateTime.UtcNow,
                    typeToLog.Name,
                    String.Format(CultureInfo.InvariantCulture, message, values));
            }
            catch (Exception ex)
            {
                return String.Format("Logging Error: an error occured while formatting your logg message from {0}. {1}", typeToLog.FullName, ex.GetFullExceptionMessage());
            }
        }
    }
}