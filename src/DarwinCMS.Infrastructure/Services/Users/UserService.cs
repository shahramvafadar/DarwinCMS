using AutoMapper;

using DarwinCMS.Application.Abstractions.Repositories;
using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Application.Services.Users;
using DarwinCMS.Domain.Entities;
using DarwinCMS.Domain.ValueObjects;
using DarwinCMS.Shared.Exceptions;
using DarwinCMS.Shared.Security;

using Microsoft.EntityFrameworkCore;

namespace DarwinCMS.Infrastructure.Services.Users;

/// <summary>
/// Implements IUserService to provide user-related business logic,
/// including creation, retrieval, update, deletion, role assignments, and list operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UserService with required dependencies.
    /// </summary>
    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(CreateUserRequest request, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByUsernameOrEmailAsync(request.Username, request.Email, cancellationToken);
        if (existing != null)
            throw new BusinessRuleException("Username or Email is already taken.");

        var validRoles = await _roleRepository.GetAllActiveAsync(cancellationToken);
        var validRoleIds = validRoles.Select(r => r.Id).ToHashSet();

        if (!request.RoleIds.All(id => validRoleIds.Contains(id)))
            throw new BusinessRuleException("One or more selected roles are invalid.");

        var hashedPassword = PasswordHasher.Hash(request.Password);

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Gender,
            request.BirthDate,
            request.Username,
            new Email(request.Email),
            hashedPassword,
            performedByUserId);

        user.SetLanguage(new LanguageCode(request.LanguageCode));
        user.SetProfilePictureUrl(request.ProfilePictureUrl);
        user.SetMobilePhone(request.MobilePhone);

        if (request.IsEmailConfirmed) user.ConfirmEmail();
        if (request.IsMobileConfirmed) user.ConfirmMobile();

        await _userRepository.AddAsync(user, cancellationToken);

        foreach (var roleId in request.RoleIds)
        {
            var userRole = new UserRole(user.Id, roleId);
            await _userRoleRepository.AddAsync(userRole, cancellationToken);
        }

        await _userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _userRepository.GetByIdAsync(id, cancellationToken);

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await _userRepository.GetByUsernameAsync(username, cancellationToken);

    /// <inheritdoc />
    public async Task DisableUserAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new BusinessRuleException("User not found.");

        user.Deactivate();
        user.SetModifiedBy(performedByUserId);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task EnableUserAsync(Guid userId, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new BusinessRuleException("User not found.");

        user.Activate();
        user.SetModifiedBy(performedByUserId);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(Guid userId, string newPassword, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new BusinessRuleException("User not found.");

        var hash = PasswordHasher.Hash(newPassword);
        user.SetPasswordHash(hash);
        user.SetModifiedBy(performedByUserId);

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<UserListResultDto> GetUserListAsync(string? search, Guid? roleId, string? sortColumn, string? sortDirection, int skip, int take, CancellationToken cancellationToken = default)
    {
        var query = _userRepository.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u =>
                u.Username.Contains(search) ||
                u.Email.Value.Contains(search) ||
                u.FirstName.Contains(search) ||
                u.LastName.Contains(search));
        }

        if (roleId.HasValue)
        {
            var userIds = await _userRoleRepository.Query()
                .Where(ur => ur.RoleId == roleId.Value)
                .Select(ur => ur.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            query = query.Where(u => userIds.Contains(u.Id));
        }

        query = sortColumn?.ToLowerInvariant() switch
        {
            "username" => sortDirection == "desc" ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
            "email" => sortDirection == "desc" ? query.OrderByDescending(u => u.Email.Value) : query.OrderBy(u => u.Email.Value),
            _ => query.OrderBy(u => u.Username)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var users = await query.Skip(skip).Take(take).ToListAsync(cancellationToken);

        var result = new List<UserListDto>();

        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserListDto>(user);
            var roles = await _userRoleRepository.GetByUserIdAsync(user.Id, null, cancellationToken);
            userDto.RoleNames = roles
                .Where(r => r.Role != null)
                .Select(r => r.Role!.DisplayName ?? r.Role.Name)
                .Distinct()
                .ToList();
            result.Add(userDto);
        }

        return new UserListResultDto
        {
            TotalCount = totalCount,
            Users = result
        };
    }

    /// <inheritdoc />
    public async Task UpdateAsync(UpdateUserRequest request, Guid performedByUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new BusinessRuleException("User not found.");

        user.SetUsername(request.Username);
        user.SetEmail(new Email(request.Email));
        user.SetFirstName(request.FirstName);
        user.SetLastName(request.LastName);
        user.SetGender(request.Gender);
        user.SetBirthDate(request.BirthDate);
        user.SetLanguage(new LanguageCode(request.LanguageCode));
        user.SetProfilePictureUrl(request.ProfilePictureUrl);
        user.SetMobilePhone(request.MobilePhone);

        if (request.IsEmailConfirmed)
            user.ConfirmEmail();
        else
            user.UnconfirmEmail();

        if (request.IsMobileConfirmed)
            user.ConfirmMobile();
        else
            user.UnconfirmMobile();

        user.SetModifiedBy(performedByUserId);
        _userRepository.Update(user);

        var userRoles = await _userRoleRepository.GetByUserIdAsync(user.Id, null, cancellationToken)
                ?? new List<UserRole>();

        foreach (var ur in userRoles.Where(r => r != null))
            _userRoleRepository.Delete(ur);


        foreach (var roleId in request.RoleIds)
        {
            await _userRoleRepository.AddAsync(new UserRole(user.Id, roleId), cancellationToken);
        }

        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new BusinessRuleException("User not found.");

        if (user.IsSystem)
            throw new BusinessRuleException("System users cannot be deleted.");

        var userRoles = await _userRoleRepository.GetByUserIdAsync(userId, null, cancellationToken);
        foreach (var ur in userRoles)
            _userRoleRepository.Delete(ur);

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
    }


    /// <inheritdoc />
    public async Task<List<Guid>> GetUserPrimaryRoleIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var roles = await _userRoleRepository.GetByUserIdAsync(userId, null, cancellationToken);
        return roles.Select(r => r.RoleId).ToList();
    }
}
