namespace DarwinCMS.Shared.Exceptions;

/// <summary>
/// Exception used for business rule violations that should be displayed to end-users in the UI.
/// These exceptions are safe to show and do not indicate system failure.
/// </summary>
public class BusinessRuleException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleException"/> class with a user-visible message.
    /// </summary>
    /// <param name="message">A safe message that can be shown to the end-user.</param>
    public BusinessRuleException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleException"/> class with a message and an inner exception.
    /// </summary>
    /// <param name="message">A safe message that can be shown to the end-user.</param>
    /// <param name="innerException">Optional inner exception for context or debugging.</param>
    public BusinessRuleException(string message, Exception innerException)
        : base(message, innerException) { }
}
