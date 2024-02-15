using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UATP.RapidPay.BusinessLogic.Interfaces;
using UATP.RapidPay.Data.Models;
using UATP.RapidPay.Data.Models.DTOs;

namespace UATP.RapidPay.Api.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardsManagementController(ICardsManagement cardsManagement, IPaymentsManagement paymentsManagement) : ControllerBase
    {
        private readonly ICardsManagement _cardsManagement = cardsManagement;
        private readonly IPaymentsManagement _paymentsManagement = paymentsManagement;
        protected APIResponse _response = new();

        [HttpPost("CreateCard")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateCard([FromBody] CardCreateDTO cardCreateDTO)
        {
            if (cardCreateDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = ["Card can not be null"];
                return BadRequest(_response);
            }
            if (await _cardsManagement.GetCardAsync(cardCreateDTO.CardNumber) != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = ["Card already exists"];
                return BadRequest(_response);
            }
            try
            {
                await _cardsManagement.CreateCardAsync(cardCreateDTO);
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

        [HttpGet("GetCardBalance")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetCardBalance(string cardNumber)
        {
            try
            {
                _response.Result = await _cardsManagement.GetCardAsync(cardNumber);
                if (_response.Result != null)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = ["Card not found."];
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors = [ex.ToString()];
            }
            return _response;
        }

        [HttpPost("PayCard")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PayCard([FromBody] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = ["Payment can not be null"];
                return BadRequest(_response);
            }
            if (await _cardsManagement.GetCardAsync(paymentDTO.CardNumber) == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors = ["Card does not exist"];
                return BadRequest(_response);
            }
            try
            {
                await _paymentsManagement.MakePayment(paymentDTO);
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
