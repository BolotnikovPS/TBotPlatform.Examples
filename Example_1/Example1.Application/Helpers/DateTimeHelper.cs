using Example1.Domain.Abstractions.Helpers;

namespace Example1.Application.Helpers;

internal class DateTimeHelper : IDateTimeHelper
{
    DateTime IDateTimeHelper.GetLocalDateTimeNow() => GetLocalDateTimeNow();

    DateTime IDateTimeHelper.GetUtcDateTimeNow() => DateTime.UtcNow;

    DateTime IDateTimeHelper.GetLocalDateNow() => GetLocalDateTimeNow().Date;

    private static DateTime GetLocalDateTimeNow() => DateTime.UtcNow.AddHours(TimeZoneInfo.FindSystemTimeZoneById("Ekaterinburg Standard Time").BaseUtcOffset.Hours);
}