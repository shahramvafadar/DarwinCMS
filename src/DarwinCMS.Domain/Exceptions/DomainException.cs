using System.Runtime.Serialization;

namespace DarwinCMS.Domain.Exceptions;

/// <summary>
/// Base exception type for all domain-level business validation errors.
/// These exceptions should be thrown only when business rules are violated,
/// not for technical or infrastructure errors.
/// </summary>
[Serializable]
public class DomainException : Exception
{
  /// <summary>
  /// A unique error code or key (useful for i18n or logging).
  /// Optional.
  /// </summary>
  public string? ErrorCode { get; }

  /// <summary>
  /// Constructs a new domain exception with message and optional code.
  /// </summary>
  public DomainException(string message, string? errorCode = null)
      : base(message)
  {
    ErrorCode = errorCode;
  }

  /// <summary>
  /// Constructs a new domain exception with message and inner exception.
  /// </summary>
  public DomainException(string message, Exception innerException)
      : base(message, innerException) { }

 
}
