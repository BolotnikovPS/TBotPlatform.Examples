using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Example1.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum ETextsType
{
    None = 0,
    IsRefreshMenu,
    MenuIsRefresh,
}