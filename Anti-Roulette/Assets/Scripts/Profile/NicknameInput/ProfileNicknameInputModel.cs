using System.Text.RegularExpressions;
using System;

public sealed class ProfileNicknameInputModel
{
    private const int MinLength = 3;
    private const int MaxLength = 17;

    // Unicode-буквы + цифры + пробел + точка + подчёркивание + дефис.
    // Пробел разрешён, но не в начале/конце (проверяется отдельно после Trim).
    private static readonly Regex AllowedRegex =
        new(@"^[\p{L}\p{N} ._-]+$", RegexOptions.Compiled);

    private static readonly Regex ConsecutiveSpacesRegex =
        new(@"\s{2,}", RegexOptions.Compiled);

    private readonly IPlayerProfileProvider _playerProfileProvider;

    private string _possibleNickname;

    public ProfileNicknameInputModel(
        IPlayerProfileProvider playerProfileProvider)
    {
        _playerProfileProvider = playerProfileProvider;
    }

    public void SetNickname(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            SetNotValid("Nickname cannot be empty");
            return;
        }

        // Обрезаем пробелы по краям — игрок не должен страдать из-за случайного пробела
        string trimmed = value.Trim();

        if (trimmed.Length < MinLength)
        {
            SetNotValid($"Nickname must be at least {MinLength} characters long");
            return;
        }

        if (trimmed.Length > MaxLength)
        {
            SetNotValid($"Nickname must not exceed {MaxLength} characters");
            return;
        }

        if (!AllowedRegex.IsMatch(trimmed))
        {
            SetNotValid("Nickname can only contain letters, numbers, spaces, periods, underscores and hyphens");
            return;
        }

        if (ConsecutiveSpacesRegex.IsMatch(trimmed))
        {
            SetNotValid("Nickname cannot contain consecutive spaces");
            return;
        }

        _possibleNickname = trimmed;

        OnSetValidate?.Invoke();
    }

    public void SubmitNickname()
    {
        if (string.IsNullOrEmpty(_possibleNickname))
            return;

        _playerProfileProvider.SetNickname(_possibleNickname);
    }

    private void SetNotValid(string error)
    {
        OnSetNotValidate?.Invoke(error);
    }

    public event Action<string> OnSetNotValidate;
    public event Action OnSetValidate;
}