using DarwinCMS.Domain.Interfaces;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Shared.Security;

namespace DarwinCMS.Domain.Entities;

/// <summary>
/// Represents a user of the system with credentials, profile, preferences, and roles.
/// </summary>
public class User : BaseEntity, IAuditableEntity
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
    /// Gender: e.g. "Male", "Female", "Other".
    /// </summary>
    public string Gender { get; private set; } = string.Empty;

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// Optional mobile phone number in international format.
    /// </summary>
    public string? MobilePhone { get; private set; }

    /// <summary>
    /// Whether the user's mobile phone is confirmed.
    /// </summary>
    public bool IsMobileConfirmed { get; private set; }

    /// <summary>
    /// Whether the user's email is confirmed.
    /// </summary>
    public bool IsEmailConfirmed { get; private set; }

    /// <summary>
    /// Unique username (URL-safe).
    /// </summary>
    public string Username { get; private set; } = string.Empty;

    /// <summary>
    /// Hashed password (never stored in plain text).
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// User's email address (used for login or communication).
    /// NOTE: Initialized with placeholder to satisfy nullable rules.
    /// </summary>
    public Email Email { get; private set; } = Email.CreatePlaceholder();

    /// <summary>
    /// Preferred language of the user (default is "en").
    /// </summary>
    public LanguageCode LanguageCode { get; private set; } = new("en");

    /// <summary>
    /// Optional profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; private set; }

    /// <summary>
    /// Whether the user is currently active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Indicates if the user is a system-critical account and cannot be deleted or disabled.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Last login timestamp (UTC).
    /// </summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>
    /// ID of the creator (admin or system).
    /// </summary>
    public Guid CreatedByUserId { get; private set; }

    /// <summary>
    /// ID of the last modifier (if applicable).
    /// </summary>
    public Guid? ModifiedByUserId { get; private set; }

    /// <summary>
    /// Navigation property for roles assigned to this user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    /// <summary>
    /// EF Core constructor (used for deserialization).
    /// </summary>
    protected User() { }

    /// <summary>
    /// Creates a new user with required fields.
    /// </summary>
    public User(
        string firstName,
        string lastName,
        string gender,
        DateTime birthDate,
        string username,
        Email email,
        string passwordHash,
        Guid createdByUserId)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetGender(gender);
        SetBirthDate(birthDate);
        SetUsername(username);
        SetEmail(email);
        SetPasswordHash(passwordHash);
        CreatedByUserId = createdByUserId;
    }

    /// <summary>
    /// Updates the first name.
    /// </summary>
    public void SetFirstName(string firstName)
    {
        FirstName = string.IsNullOrWhiteSpace(firstName)
            ? throw new ArgumentException("Required", nameof(firstName))
            : firstName.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Updates the last name.
    /// </summary>
    public void SetLastName(string lastName)
    {
        LastName = string.IsNullOrWhiteSpace(lastName)
            ? throw new ArgumentException("Required", nameof(lastName))
            : lastName.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Updates gender string.
    /// </summary>
    public void SetGender(string gender)
    {
        Gender = string.IsNullOrWhiteSpace(gender)
            ? throw new ArgumentException("Required", nameof(gender))
            : gender.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Sets the date of birth (cannot be future).
    /// </summary>
    public void SetBirthDate(DateTime birthDate)
    {
        if (birthDate > DateTime.UtcNow)
            throw new ArgumentException("Birth date cannot be in the future.", nameof(birthDate));

        BirthDate = birthDate;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the mobile phone number.
    /// </summary>
    public void SetMobilePhone(string? phone)
    {
        MobilePhone = phone?.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Marks the mobile as confirmed.
    /// </summary>
    public void ConfirmMobile() => IsMobileConfirmed = true;

    /// <summary>
    /// Marks the mobile as unconfirmed.
    /// </summary>
    public void UnconfirmMobile() => IsMobileConfirmed = false;

    /// <summary>
    /// Marks the email as confirmed.
    /// </summary>
    public void ConfirmEmail() => IsEmailConfirmed = true;

    /// <summary>
    /// Marks the email as unconfirmed.
    /// </summary>
    public void UnconfirmEmail() => IsEmailConfirmed = false;


    /// <summary>
    /// Updates the username (stored in lowercase).
    /// </summary>
    public void SetUsername(string username)
    {
        Username = string.IsNullOrWhiteSpace(username)
            ? throw new ArgumentException("Required", nameof(username))
            : username.Trim().ToLowerInvariant();
        MarkAsModified();
    }

    /// <summary>
    /// Sets the user's email value object.
    /// </summary>
    public void SetEmail(Email email)
    {
        Email = email;
        MarkAsModified();
    }

    /// <summary>
    /// Sets a hashed password directly (no hashing).
    /// </summary>
    public void SetPasswordHash(string hash)
    {
        PasswordHash = string.IsNullOrWhiteSpace(hash)
            ? throw new ArgumentException("Required", nameof(hash))
            : hash.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Sets a new hashed password using the system password hasher.
    /// </summary>
    public void SetPassword(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentException("Password cannot be empty.", nameof(plainTextPassword));

        PasswordHash = PasswordHasher.Hash(plainTextPassword);
        MarkAsModified();
    }

    /// <summary>
    /// Sets the user's preferred language.
    /// </summary>
    public void SetLanguage(LanguageCode code)
    {
        LanguageCode = code;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the profile picture URL.
    /// </summary>
    public void SetProfilePictureUrl(string? url)
    {
        ProfilePictureUrl = url?.Trim();
        MarkAsModified();
    }

    /// <summary>
    /// Marks this user as a system-critical account.
    /// </summary>
    public void MarkAsSystem()
    {
        IsSystem = true;
        MarkAsModified();
    }


    /// <summary>
    /// Records the last login timestamp.
    /// </summary>
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsModified();
    }

    /// <summary>
    /// Deactivates the user.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        MarkAsModified();
    }

    /// <summary>
    /// Reactivates the user.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
        MarkAsModified();
    }

    /// <summary>
    /// Sets the last modified user ID.
    /// </summary>
    public void SetModifiedBy(Guid modifierId)
    {
        ModifiedByUserId = modifierId;
        MarkAsModified();
    }


    /// <summary>
    /// Returns formatted string summary of the user.
    /// </summary>
    public override string ToString() => $"{Username} ({FullName}) <{Email}>";
}
