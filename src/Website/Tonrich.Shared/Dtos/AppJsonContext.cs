﻿
using Tonrich.Shared.Dtos.Account;
using Tonrich.Shared.Dtos.TonApi;

namespace Tonrich.Shared.Dtos;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(SignInRequestDto))]
[JsonSerializable(typeof(RestErrorInfo))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(Account.TokenResponseDto))]
[JsonSerializable(typeof(AccountInfo))]
[JsonSerializable(typeof(BulkAccountInfo))]
[JsonSerializable(typeof(AccountNFT))]
[JsonSerializable(typeof(AccountEvents))]
[JsonSerializable(typeof(AccountTransactions))]
[JsonSerializable(typeof(Dictionary<string, object>))]

public partial class AppJsonContext : JsonSerializerContext
{
}
