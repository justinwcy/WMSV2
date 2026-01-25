using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OrganizationService.Constants;
using OrganizationService.DTOs;
using OrganizationService.Mappings;
using OrganizationService.Models;
using OrganizationService.Repositories;
using OrganizationService.Service;

using WMSCommon.Constants;
using WMSCommon.Results;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/v1/OrganizationService/[controller]")]
    public class StaffsController(
        ITokenService tokenService,
        IStaffRepository staffRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetStaffById")]
        public async Task<ActionResult<StaffReadDTO>> GetStaffById(Guid id)
        {
            var staff = await staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return Ok(staff.ToReadDTO());
        }

        [HttpPost("Register")]
        public async Task<ActionResult<StaffReadDTO>> Register(StaffRegisterDTO staffRegisterDTO)
        {
            var staff = staffRegisterDTO.ToModel();
            var result = await staffRepository.CreateAsync(staff);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetStaffById),
                new { Id = staff.Id }, staff.ToReadDTO());
        }

        [HttpPost("Login")]
        public async Task<ActionResult<StaffReadDTO>> Login(StaffLoginDTO staffLoginDTO)
        {
            var result = await staffRepository.Login(staffLoginDTO.Email, staffLoginDTO.Password);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            var staff = await staffRepository.GetUserByEmail(staffLoginDTO.Email);
            var token = await tokenService.CreateToken(staff);
            var refreshToken = await tokenService.CreateRefreshToken(staff);

            var roles = await staffRepository.GetRoles(staff);
            var userReadDTO = staff.ToReadDTO();

            // Set the tokens as cookies
            SetTokenCookies(token, refreshToken);

            return Ok(userReadDTO);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<StaffReadDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies[Token.RefreshToken];

            var staffId = await tokenService.GetUserIdFromRefreshToken(
                refreshToken);
            if (staffId == Guid.Empty)
            {
                return Unauthorized("Invalid refresh token");
            }

            var staff = await staffRepository.GetByIdAsync(staffId);
            if (staff == null)
            {
                return BadRequest("User not found");
            }

            var token = await tokenService.CreateToken(staff);
            var newRefreshToken = await tokenService.CreateRefreshToken(staff);

            var roles = await staffRepository.GetRoles(staff);
            var userReadDTO = staff.ToReadDTO();

            // Set the tokens as cookies
            SetTokenCookies(token, newRefreshToken);

            return Ok(userReadDTO);
        }

        [HttpGet("Roles")]
        public ActionResult<IEnumerable<string>> GetAllRoles()
        {
            return Ok(staffRepository.GetAllRoles());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<StaffReadDTO>> Update(
            Guid id,
            StaffUpdateDTO staffUpdateDTO)
        {
            Staff staff = staffUpdateDTO.ToModel();
            staff.Id = id;

            RepositoryResult<Staff> updateUserResult = await staffRepository.UpdateAsync(staff);
            if (!updateUserResult.IsSuccess)
            {
                return StatusCode(500, updateUserResult.Message);
            }

            RepositoryResult<Staff> updateRoleResult = await staffRepository.UpdateUserRoles(
                id, staffUpdateDTO.RoleIds);
            if (!updateRoleResult.IsSuccess)
            {
                return StatusCode(500, updateRoleResult.Message);
            }

            var token = await tokenService.CreateToken(staff);
            var refreshToken = await tokenService.CreateRefreshToken(staff);

            var roles = await staffRepository.GetRoles(staff);
            var userReadDTO = staff.ToReadDTO();

            // Set the tokens as cookies
            SetTokenCookies(token, refreshToken);

            return Ok(userReadDTO);
        }

        [Authorize]
        [HttpPost("ChangePassword/{userId}")]
        public async Task<ActionResult> ChangeUserPassword(
            string userId,
            StaffChangePasswordDTO staffChangePasswordDTO)
        {
            var result = await staffRepository.ChangePassword(
                userId,
                staffChangePasswordDTO.OldPassword,
                staffChangePasswordDTO.NewPassword);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<StaffReadDTO>>> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var staffs = await staffRepository.GetAsync(pageSize, pageNumber);
            int staffCount = await staffRepository.CountAsync();
            var userReadDTOs = new List<StaffReadDTO>();
            foreach (var staff in staffs)
            {
                var userReadDTO = staff.ToReadDTO();
                userReadDTOs.Add(userReadDTO);
            }

            var result = new PaginationResult<StaffReadDTO>
            {
                Items = userReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = staffCount
            };

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await staffRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound(result);
            }
            return NoContent();
        }

        [Authorize]
        [HttpPost("Logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete(Token.AccessToken);
            Response.Cookies.Delete(Token.RefreshToken);

            return NoContent();
        }

        private void SetTokenCookies(string token, string refreshToken)
        {
            var tokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Domain = ".wms.com",
                Expires = DateTime.UtcNow.AddSeconds(Token.AccessTokenExpiryTime)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                Domain = ".wms.com",
                Expires = DateTime.UtcNow.AddSeconds(Token.RefreshTokenExpiryTime)
            };

            // Set the cookies in the response
            Response.Cookies.Append(Token.AccessToken, token, tokenOptions);
            Response.Cookies.Append(Token.RefreshToken, refreshToken, refreshTokenOptions);
        }
    }
}
