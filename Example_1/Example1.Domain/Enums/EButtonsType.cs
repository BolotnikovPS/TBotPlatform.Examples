using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Example1.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum EButtonsType
{
    None = 0,
    ToBackMain,
    ToBack,
    Admin,
    ListJobs,
    GetUsersStatistic,
    GetAllUsers,
    RefreshMenu,
}