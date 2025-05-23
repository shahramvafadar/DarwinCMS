# Contributing to DarwinCMS

We welcome contributions to DarwinCMS, whether it's bug reports, feature suggestions, documentation, or code improvements.

## 🧩 How to Contribute

1. **Fork the repository** and clone it locally.
2. Create a new branch: `git checkout -b feature/my-feature-name`
3. Make your changes with clear commit messages.
4. Run tests locally: `dotnet test`
5. Submit a pull request (PR) with a clear description of your changes.

## 🧪 Testing

Ensure your code includes:
- Unit tests in `DarwinCMS.UnitTests`
- Integration tests in `DarwinCMS.IntegrationTests` when needed
- Use `ITestOutputHelper` for detailed outputs
- Tests should follow `Arrange-Act-Assert` style

## 🎯 Guidelines

- Follow Clean Architecture and SOLID principles
- Avoid adding any paid or commercial packages
- Use open-source tools and libraries
- All new APIs must use Web API Controllers (not Minimal APIs)
- Follow the structure outlined in `PROJECT_NOTES.md`

## 🗣️ Communication

- If in doubt, open an issue before starting work
- Mention the issue number in your pull request

Thank you for helping improve DarwinCMS!
