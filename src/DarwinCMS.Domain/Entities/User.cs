using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Shared.Security;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a user of the system with profile, credentials, preferences, and security state.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// First name of the user.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>
    /// Computed full name (FirstName + LastName).
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Gender of the user ("Male", "Female", "Other").
    /// </summary>
    public string Gender { get; private set; } = string.Empty;

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// Optional mobile phone number.
    /// </summary>
    public string? MobilePhone { get; private set; }

    /// <summary>
    /// Whether the user's mobile phone has been confirmed.
    /// </summary>
    public bool IsMobileConfirmed { get; private set; }

    /// <summary>
    /// Whether the user's email has been confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; private set; }

    /// <summary>
    /// Unique username.
    /// </summary>
    public string Username { get; private set; } = string.Empty;

    /// <summary>
    /// Hashed password.
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// User's email address (as a value object).
    /// </summary>
    public Email Email { get; private set; } = Email.CreatePlaceholder();

    /// <summary>
    /// Preferred language of the user.
    /// </summary>
    public LanguageCode LanguageCode { get; private set; } = new("en");

    /// <summary>
    /// Optional profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; private set; }

    /// <summary>
    /// Whether the user is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Whether the user is a system-critical account.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Timestamp of last login.
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// Roles assigned to this user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// EF Core constructor.
    /// </summary>
    protected User() { }

    /// <summary>
    /// Creates a new user with required fields.
    /// </summary>
    public User(string firstName, string lastName, string gender, DateTime birthDate, string username, Email email, string passwordHash, Guid createdByUserId)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetGender(gender);
        SetBirthDate(birthDate);
        SetUsername(username);
        SetEmail(email);
        SetPasswordHash(passwordHash);
        MarkAsCreated(createdByUserId);
    }

    /// <summary>
    /// Updates the first name.
    /// </summary>
    public void SetFirstName(string firstName)
    {
        FirstName = string.IsNullOrWhiteSpace(firstName) ? throw new ArgumentException("First name is required.", nameof(firstName)) : firstName.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Updates the last name.
    /// </summary>
    public void SetLastName(string lastName)
    {
        LastName = string.IsNullOrWhiteSpace(lastName) ? throw new ArgumentException("Last name is required.", nameof(lastName)) : lastName.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Updates the gender.
    /// </summary>
    public void SetGender(string gender)
    {
        Gender = string.IsNullOrWhiteSpace(gender) ? throw new ArgumentException("Gender is required.", nameof(gender)) : gender.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the date of birth.
    /// </summary>
    public void SetBirthDate(DateTime birthDate)
    {
        if (birthDate > DateTime.UtcNow)
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));
        BirthDate = birthDate;
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the mobile phone.
    /// </summary>
    public void SetMobilePhone(string? phone)
    {
        MobilePhone = phone?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Confirms the mobile number.
    /// </summary>
    public void ConfirmMobile() => IsMobileConfirmed = true;

    /// <summary>
    /// Unconfirms the mobile number.
    /// </summary>
    public void UnconfirmMobile() => IsMobileConfirmed = false;

    /// <summary>
    /// Confirms the email address.
    /// </summary>
    public void ConfirmEmail() => IsEmailConfirmed = true;

    /// <summary>
    /// Unconfirms the email address.
    /// </summary>
    public void UnconfirmEmail() => IsEmailConfirmed = false;

    /// <summary>
    /// Updates the username.
    /// </summary>
    public void SetUsername(string username)
    {
        Username = string.IsNullOrWhiteSpace(username) ? throw new ArgumentException("Username is required.", nameof(username)) : username.Trim().ToLowerInvariant();
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the email.
    /// </summary>
    public void SetEmail(Email email)
    {
        Email = email;
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the password hash.
    /// </summary>
    public void SetPasswordHash(string hash)
    {
        PasswordHash = string.IsNullOrWhiteSpace(hash) ? throw new ArgumentException("Password hash is required.", nameof(hash)) : hash.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Hashes and sets a new password.
    /// </summary>
    public void SetPassword(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentException("Password cannot be empty.", nameof(plainTextPassword));
        PasswordHash = PasswordHasher.Hash(plainTextPassword);
        MarkAsModified(null);
    }

    /// <summary>
    /// Updates the preferred language.
    /// </summary>
    public void SetLanguage(LanguageCode code)
    {
        LanguageCode = code;
        MarkAsModified(null);
    }

    /// <summary>
    /// Sets the profile picture URL.
    /// </summary>
    public void SetProfilePictureUrl(string? url)
    {
        ProfilePictureUrl = url?.Trim();
        MarkAsModified(null);
    }

    /// <summary>
    /// Marks the user as a system account.
    /// </summary>
    public void MarkAsSystem()
    {
        IsSystem = true;
        MarkAsModified(null);
    }

    /// <summary>
    /// Records the user's last login timestamp.
    /// </summary>
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsModified(null);
    }

    /// <summary>
    /// Deactivates the user (soft delete).
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        MarkAsModified(null);
    }

    /// <summary>
    /// Reactivates the user.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        MarkAsModified(null);
    }

    /// <summary>
    /// Marks the user as logically deleted.
    /// </summary>
    public void MarkAsDeleted(Guid? modifierId = null)
    {
        IsDeleted = true;
        MarkAsModified(modifierId, isDeleted: true);
    }

    /// <summary>
    /// Restores the user from soft-deleted state.
    /// </summary>
    public void Restore(Guid? modifierId = null)
    {
        IsDeleted = false;
        MarkAsModified(modifierId, isDeleted: false);
    }

    /// <summary>
    /// Updates the last modifier user ID.
    /// </summary>
    public void SetModifiedBy(Guid modifierId)
    {
        ModifiedByUserId = modifierId;
        MarkAsModified(modifierId);
    }

    /// <summary>
    /// Returns a formatted string summary.
    /// </summary>
    public override string ToString() => $"{Username} ({FullName}) <{Email}>";
}
