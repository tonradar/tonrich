using System.Runtime.Serialization;

namespace Tonrich.Shared.Dtos.TonApi.Enum;

public enum EventStatus
{
    [EnumMember(Value = "ok")]
    Ok,

    [EnumMember(Value = "failed")]
    Failed,

    [EnumMember(Value = "pending")]
    Pending
}
