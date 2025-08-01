using TechpertsSolutions.Core.DTOs;
using TechpertsSolutions.Core.DTOs.Login;
using TechpertsSolutions.Core.Entities;
using System.Collections.Generic;

namespace Service.Utilities
{
    public static class AuthMapper
    {
        public static GeneralResponse<LoginResultDTO> MapToLoginResponse(bool success, string message, LoginResultDTO data)
        {
            return new GeneralResponse<LoginResultDTO>
            {
                Success = success,
                Message = message,
                Data = data
            };
        }

        public static GeneralResponse<string> MapToAuthResponse(bool success, string message, string data)
        {
            return new GeneralResponse<string>
            {
                Success = success,
                Message = message,
                Data = data
            };
        }

        public static LoginResultDTO MapToLoginResultDTO(AppUser user, string token, List<string> roles)
        {
            if (user == null) return null;

            return new LoginResultDTO
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName,
                RoleName = roles
            };
        }

        public static GeneralResponse<string> MapToDeleteAccountResponse(bool success, string message, string email)
        {
            return new GeneralResponse<string>
            {
                Success = success,
                Message = message,
                Data = email
            };
        }
    }
} 