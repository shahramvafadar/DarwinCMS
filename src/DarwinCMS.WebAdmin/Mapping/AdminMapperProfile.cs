using AutoMapper;

using DarwinCMS.Application.DTOs.Menus;
using DarwinCMS.Application.DTOs.Pages;
using DarwinCMS.Application.DTOs.Permissions;
using DarwinCMS.Application.DTOs.Roles;
using DarwinCMS.Application.DTOs.Users;
using DarwinCMS.Application.DTOs.SiteSettings;

using DarwinCMS.Domain.Entities;

using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Pages;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Permissions;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Roles;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Users;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.Menus;
using DarwinCMS.WebAdmin.Areas.Admin.ViewModels.SiteSettings;

namespace DarwinCMS.WebAdmin.Mapping;

/// <summary>
/// AutoMapper profile for mapping between Admin ViewModels and Application DTOs.
/// This ensures clean separation of concerns between the UI and service layers.
/// </summary>
public class AdminMapperProfile : Profile
{
    /// <summary>
    /// Initializes mapping rules for all admin UI ViewModels and service DTOs.
    /// </summary>
    public AdminMapperProfile()
    {
        // ========== USER MAPPINGS ==========
        CreateMap<CreateUserViewModel, CreateUserRequest>();
        CreateMap<EditUserViewModel, UpdateUserRequest>();

        CreateMap<User, EditUserViewModel>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode.Value))
            .ForMember(dest => dest.RoleIds, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap<User, UserListDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        // ========== ROLE MAPPINGS ==========
        CreateMap<CreateRoleViewModel, CreateRoleRequest>();
        CreateMap<EditRoleViewModel, UpdateRoleRequest>();

        CreateMap<RoleDto, EditRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.RawDisplayName));
        CreateMap<RoleDto, RoleListItemViewModel>();
        CreateMap<RoleDto, DeleteRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.RawDisplayName, opt => opt.MapFrom(src => src.DisplayName));

        CreateMap<Role, EditRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<Role, DeleteRoleViewModel>()
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        // ========== PERMISSION MAPPINGS ==========
        CreateMap<Permission, PermissionListDto>();
        CreateMap<CreatePermissionViewModel, CreatePermissionRequest>();
        CreateMap<EditPermissionViewModel, UpdatePermissionRequest>();
        CreateMap<Permission, EditPermissionViewModel>();

        // ========== PAGE MAPPINGS ==========
        CreateMap<Page, PageListItemDto>();
        CreateMap<Page, PageDetailDto>();
        CreateMap<CreatePageDto, Page>();
        CreateMap<UpdatePageDto, Page>().ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<PageListItemDto, PageListItemViewModel>();
        CreateMap<PageDetailDto, PageListItemViewModel>();
        CreateMap<PageDetailDto, EditPageViewModel>();
        CreateMap<CreatePageViewModel, CreatePageDto>();
        CreateMap<EditPageViewModel, UpdatePageDto>();

        // ========== MENU MAPPINGS ==========
        CreateMap<Menu, MenuListItemDto>();
        CreateMap<Menu, MenuDetailDto>();
        CreateMap<CreateMenuDto, Menu>();
        CreateMap<UpdateMenuDto, Menu>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<MenuListItemDto, MenuListItemViewModel>();
        CreateMap<MenuDetailDto, EditMenuViewModel>();
        CreateMap<MenuDetailDto, DeleteMenuViewModel>();
        CreateMap<CreateMenuViewModel, CreateMenuDto>();
        CreateMap<EditMenuViewModel, UpdateMenuDto>();

        // ========== MENU ITEM MAPPINGS ==========
        CreateMap<MenuItem, MenuItemDto>();
        CreateMap<MenuItemDto, MenuItemListItemViewModel>();
        CreateMap<PageDetailDto, PageListItemViewModel>();
        CreateMap<CreateMenuItemViewModel, CreateMenuItemDto>();
        CreateMap<EditMenuItemViewModel, UpdateMenuItemDto>();
        CreateMap<MenuItemDto, EditMenuItemViewModel>();
        CreateMap<CreateMenuItemDto, MenuItem>();
        CreateMap<UpdateMenuItemDto, MenuItem>();


        // ========== SITE SETTING MAPPINGS ==========
        CreateMap<SiteSetting, SiteSettingListDto>();
        CreateMap<SiteSettingListDto, SiteSettingListViewModel>().ReverseMap();
        CreateMap<SiteSetting, SiteSettingListViewModel>();
        CreateMap<SiteSetting, SiteSettingEditViewModel>().ReverseMap();

        CreateMap<SiteSettingEditViewModel, CreateSiteSettingRequest>().ReverseMap();
        CreateMap<SiteSettingEditViewModel, UpdateSiteSettingRequest>().ReverseMap();
        CreateMap<CreateSiteSettingRequest, SiteSetting>();

    }
}
