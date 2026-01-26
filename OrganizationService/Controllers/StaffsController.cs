using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OrganizationService.Constants;
using OrganizationService.DTOs;
using OrganizationService.Mappings;
using OrganizationService.Models;
using OrganizationService.Repositories;
using OrganizationService.Results;
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
            var result = await staffRepository.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return Ok(result.User.ToReadDTO(result.Roles));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<StaffReadDTO>> Register(StaffRegisterDTO staffRegisterDTO)
        {
            var staff = staffRegisterDTO.ToModel();
            var result = await staffRepository.RegisterAsync(
                staff, 
                staffRegisterDTO.Password, 
                staffRegisterDTO.Roles);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetStaffById),
                new { Id = result.User.Id }, result.User.ToReadDTO(result.Roles));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<StaffReadDTO>> Login(StaffLoginDTO staffLoginDTO)
        {
            var result = await staffRepository.LoginAsync(staffLoginDTO.Email, staffLoginDTO.Password);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            var staff = result.User;
            var token = await tokenService.CreateToken(staff);
            var refreshToken = await tokenService.CreateRefreshToken(staff);
            // Set the tokens as cookies
            SetTokenCookies(token, refreshToken);

            var userReadDTO = staff.ToReadDTO(result.Roles);
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

            var result = await staffRepository.GetByIdAsync(staffId);
            if (result.User == null)
            {
                return BadRequest("User not found");
            }

            var token = await tokenService.CreateToken(result.User);
            var newRefreshToken = await tokenService.CreateRefreshToken(result.User);

            var userReadDTO = result.User.ToReadDTO(result.Roles);

            // Set the tokens as cookies
            SetTokenCookies(token, newRefreshToken);

            return Ok(userReadDTO);
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<IEnumerable<IdentityRole<Guid>>>> GetAllRoles()
        {
            return Ok(await staffRepository.GetAllRolesAsync());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<StaffReadDTO>> Update(
            Guid id,
            StaffUpdateDTO staffUpdateDTO)
        {
            Staff staff = staffUpdateDTO.ToModel();
            staff.Id = id;

            UserResult updateUserResult = await staffRepository.UpdateAsync(
                staff, 
                staffUpdateDTO.Roles);
            if (!updateUserResult.IsSuccess)
            {
                return StatusCode(500, updateUserResult.Message);
            }

            var token = await tokenService.CreateToken(staff);
            var refreshToken = await tokenService.CreateRefreshToken(staff);
            var userReadDTO = staff.ToReadDTO(updateUserResult.Roles);

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
            var result = await staffRepository.ChangePasswordAsync(
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
            var results = await staffRepository.GetAsync(pageSize, pageNumber);
            int staffCount = await staffRepository.CountAsync();
            var staffReadDTOs = new List<StaffReadDTO>();
            foreach (var userResult in results)
            {
                var staffReadDTO = userResult.User.ToReadDTO(userResult.Roles);
                staffReadDTOs.Add(staffReadDTO);
            }

            var result = new PaginationResult<StaffReadDTO>
            {
                Items = staffReadDTOs,
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
                //Domain = ".wms.com",
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddSeconds(Token.AccessTokenExpiryTime)
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/",
                //Domain = ".wms.com",
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddSeconds(Token.RefreshTokenExpiryTime)
            };

            // Set the cookies in the response
            Response.Cookies.Append(Token.AccessToken, token, tokenOptions);
            Response.Cookies.Append(Token.RefreshToken, refreshToken, refreshTokenOptions);
        }
    }
}
