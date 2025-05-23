# Contributing to DarwinCMS

We welcome all contributions — from bug reports and feature suggestions to pull requests and documentation updates.

## 🚀 How to Contribute

1. **Fork the repository** and clone your copy.
2. Create a new branch using a clear naming convention:
   - `feature/module-name`
   - `bugfix/issue-description`
3. Make your changes with clean, well-commented code.
4. **Run tests** before submitting:
   ```bash
   dotnet test
   ```
5. **Commit your changes** using [Conventional Commits](https://www.conventionalcommits.org/):
   ```
   feat: add localization module support
   fix: resolve navigation caching issue
   ```
6. **Open a Pull Request**, referencing the related issue (if applicable).

## 🧪 Testing

- Unit tests go in `DarwinCMS.UnitTests`
- Integration tests in `DarwinCMS.IntegrationTests`
- Use `FluentAssertions`, `AutoFixture`, `Moq`, and `ITestOutputHelper`
- Follow the Arrange-Act-Assert structure

## 💡 Best Practices

- Follow Clean Architecture and SOLID principles
- Use interfaces for all dependencies
- Avoid direct database access in application layer
- Write XML documentation for all public methods and properties

## 🙋 Questions?

Open an issue if you’re unsure about your approach before writing code.

Thank you for contributing to DarwinCMS!
