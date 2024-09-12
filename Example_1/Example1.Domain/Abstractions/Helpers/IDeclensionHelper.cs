using Example1.Domain.Enums;

namespace Example1.Domain.Abstractions.Helpers;

public interface IDeclensionHelper
{
    string Decline(int value, EDeclensionType type);
}