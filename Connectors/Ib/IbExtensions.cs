namespace Connectors.Ib;

using System;
using System.Globalization;

public static class IbExtensions
{
    public static DateTime ToDateTime(this string ibDate) => DateTime.ParseExact(ibDate, "yyyyMMdd", CultureInfo.CurrentCulture);
    
}