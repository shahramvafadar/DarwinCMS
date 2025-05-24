using System;

using DarwinCMS.Domain.ValueObjects;

using FluentAssertions;

using Xunit;

namespace DarwinCMS.UnitTests.Domain.ValueObjects;

/// <summary>
/// Unit tests for the Email value object, covering validation and equality checks.
/// </summary>
public class EmailTests
{
    /// <summary>
    /// Valid email address should be accepted.
    /// </summary>
    [Theory(DisplayName = "Should create valid email object")]
    [InlineData("user@example.com")]
    [InlineData("john.doe@company.org")]
    [InlineData("test123@sub.domain.net")]
    public void Constructor_Should_Create_Email_When_Valid(string input)
    {
        var email = new Email(input);
        email.Value.Should().Be(input.ToLowerInvariant());
    }

    /// <summary>
    /// Handles empty and malformed email strings
    /// </summary>
    [Theory(DisplayName = "Should throw when email format is invalid")]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData("missing@domain")]
    [InlineData("@no-prefix.com")]
    public void Constructor_Should_Throw_When_Invalid(string input)
    {
        Action act = () => new Email(input);
        act.Should().Throw<ArgumentException>();
    }

    /// <summary>
    /// Handles null specifically
    /// </summary>
    [Fact(DisplayName = "Should throw when email is null")]
    public void Constructor_Should_Throw_When_Null()
    {
        Action act = () => new Email(null!);
        act.Should().Throw<ArgumentException>();
    }


    /// <summary>
    /// Two emails with same value should be equal.
    /// </summary>
    [Fact(DisplayName = "Should compare emails by value")]
    public void Equals_Should_Return_True_When_Emails_Match()
    {
        var email1 = new Email("same@example.com");
        var email2 = new Email("same@example.com");

        email1.Equals(email2).Should().BeTrue();
        (email1 == email2).Should().BeTrue();
    }

    /// <summary>
    /// Emails with different values should not be equal.
    /// </summary>
    [Fact(DisplayName = "Should detect inequality when values differ")]
    public void Equals_Should_Return_False_When_Emails_Differ()
    {
        var email1 = new Email("first@example.com");
        var email2 = new Email("second@example.com");

        email1.Equals(email2).Should().BeFalse();
        (email1 != email2).Should().BeTrue();
    }

    /// <summary>
    /// Placeholder email should be valid and comparable.
    /// </summary>
    [Fact(DisplayName = "Should create placeholder email")]
    public void CreatePlaceholder_Should_Return_Valid_Email()
    {
        var placeholder = Email.CreatePlaceholder();

        placeholder.Value.Should().Be("placeholder@system.local");
        placeholder.ToString().Should().Be("placeholder@system.local");
    }
}
