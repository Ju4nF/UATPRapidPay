using Microsoft.AspNetCore.Mvc;
using System.Net;
using UATP.RapidPay.BusinessLogic.Interfaces;
using UATP.RapidPay.Data.Models;
using UATP.RapidPay.Data.Models.DTOs;

namespace UATP.RapidPay.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersManagementController(IUsersManagement userManagement) : ControllerBase
    {
        private readonly IUsersManagement _userManagement = userManagement;
        protected APIResponse _response = new();

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponseDTO = await _userManagement.LoginAsync(loginRequestDTO);
            if (loginResponseDTO.User == null || string.IsNullOrEmpty(loginResponseDTO.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors = ["Username or password is incorrect"];
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponseDTO;
            return Ok(_response);
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            bool userExists = await _userManagement.IsUniqueUserAsync(registrationRequestDTO.UserName);
            if (userExists)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors = ["Username already exists"];
                return BadRequest(_response);
            }

            try
            {
                await _userManagement.RegisterAsync(registrationRequestDTO);
                _response.StatusCode = HttpStatusCode.Created;
                return Created("", _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = [ex.ToString()];
            }
            return _response;
        }
    }
}
