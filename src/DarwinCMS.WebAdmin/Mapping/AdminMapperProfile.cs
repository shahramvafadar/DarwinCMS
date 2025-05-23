using AutoMapper;
using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Domain.Entities;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;

namespace DarwinCMS.WebAdmin.Mapping;

/// <summary>
/// AutoMapper profile for mapping between Admin ViewModels and Application DTOs.
/// This ensures clean separation of concerns between the UI and service layers.
/// </summary>
public class AdminMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminMapperProfile"/> class.
    /// All mappings between domain, DTOs, and view models are defined here.
    /// </summary>
    public AdminMapperProfile()
    {
        // ========== USER MAPPINGS ==========

        // Maps CreateUserViewModel to CreateUserRequest for user creation.
        CreateMap<CreateUserViewModel, CreateUserRequest>();

        // Maps EditUserViewModel to UpdateUserRequest for user updates.
        CreateMap<EditUserViewModel, UpdateUserRequest>();

        // Maps User entity to EditUserViewModel.
        // ValueObjects like Email and LanguageCode are mapped to their string values.
        // RoleIds and Roles are manually populated in the controller.
        CreateMap<User, EditUserViewModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode.Value))
            .ForMember(dest => dest.RoleIds, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        // Maps User entity to UserListDto used in table listing.
        // RoleNames are manually populated later.
        CreateMap<User, UserListDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        // ========== ROLE MAPPINGS ==========

        // Maps CreateRoleViewModel to CreateRoleRequest.
        CreateMap<CreateRoleViewModel, CreateRoleRequest>();

        // Maps EditRoleViewModel to UpdateRoleRequest.
        CreateMap<EditRoleViewModel, UpdateRoleRequest>();

        // Maps RoleDto to EditRoleViewModel for editing roles.
        CreateMap<RoleDto, EditRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.RawDisplayName));

        // Maps RoleDto to RoleListItemViewModel for listing roles in the index table.
        CreateMap<RoleDto, RoleListItemViewModel>();

        // Maps RoleDto to DeleteRoleViewModel used in delete confirmation page.
        CreateMap<RoleDto, DeleteRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

        // Maps Role entity to RoleDto (used by application layer and UI).
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.RawDisplayName, opt => opt.MapFrom(src => src.DisplayName));

        // Fallback: Maps Role entity to EditRoleViewModel when RoleDto is not used.
        CreateMap<Role, EditRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        // Fallback: Maps Role entity to DeleteRoleViewModel when RoleDto is not used.
        CreateMap<Role, DeleteRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
