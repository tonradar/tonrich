﻿namespace Tonrich.Shared.Services.Contracts;

public interface IDateTimeProvider
{
    DateTimeOffset GetCurrentDateTime();
}