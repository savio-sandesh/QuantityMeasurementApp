using BusinessLayer;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementWebApi.Models;
using QuantityMeasurementWebApi.Data;
using BusinessQuantityDTO = ModelLayer.QuantityDTO;
using BusinessQuantityMeasurementDTO = ModelLayer.QuantityMeasurementDTO;

namespace QuantityMeasurementWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    [Authorize]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;
        private readonly QuantityDbContext _dbContext;

        public QuantityMeasurementController(IQuantityMeasurementService service, QuantityDbContext dbContext)
        {
            _service = service;
            _dbContext = dbContext;
        }

        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputDTO? input)
        {
            try
            {
                EnsureQuantityInput(input, requireTargetQuantity: false, operationName: "comparison");

                int userId = GetCurrentUserId();
                var result = _service.Compare(ToBusinessDto(input!.ThisQuantityDTO), ToBusinessDto(input.ThatQuantityDTO), userId);
                return Ok(ToApiDto(result));
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("convert")]
        public IActionResult Convert([FromBody] QuantityInputDTO? input)
        {
            try
            {
                EnsureQuantityInput(input, requireTargetQuantity: true, operationName: "conversion");

                int userId = GetCurrentUserId();
                var result = _service.Convert(ToBusinessDto(input!.ThisQuantityDTO), input.TargetQuantityDTO!.Unit, userId);
                return Ok(ToApiDto(result));
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityInputDTO? input)
        {
            try
            {
                EnsureQuantityInput(input, requireTargetQuantity: true, operationName: "addition");

                int userId = GetCurrentUserId();
                var result = _service.Add(
                    ToBusinessDto(input!.ThisQuantityDTO),
                    ToBusinessDto(input.ThatQuantityDTO),
                    input.TargetQuantityDTO!.Unit,
                    userId);

                return Ok(ToApiDto(result));
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityInputDTO? input)
        {
            try
            {
                EnsureQuantityInput(input, requireTargetQuantity: true, operationName: "subtraction");

                var result = _service.Subtract(
                    ToBusinessDto(input!.ThisQuantityDTO),
                    ToBusinessDto(input.ThatQuantityDTO),
                    input.TargetQuantityDTO!.Unit,
                    GetCurrentUserId());

                return Ok(ToApiDto(result));
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("divide")]
        public IActionResult Divide([FromBody] MathRequestDTO? request)
        {
            try
            {
                var requestValidationErrors = ValidateDivideRequest(request);
                if (requestValidationErrors.Count > 0)
                {
                    return BadRequest(new
                    {
                        message = "Invalid divide request payload.",
                        errors = requestValidationErrors
                    });
                }

                MathRequestDTO safeRequest = request!;
                string measurementType = safeRequest.MeasurementType!.Trim();
                string targetUnit = safeRequest.TargetUnit?.Trim() ?? string.Empty;

                if (safeRequest.TargetUnit is not null && string.IsNullOrWhiteSpace(targetUnit))
                {
                    throw new QuantityMeasurementException("TargetUnit cannot be empty when provided.");
                }

                int userId = GetCurrentUserId();
                var result = _service.Divide(
                    ToBusinessDto(safeRequest.FirstQuantityDTO!, measurementType),
                    ToBusinessDto(safeRequest.SecondQuantityDTO!, measurementType),
                    userId,
                    targetUnit);

                return Ok(ToApiDto(result, targetUnit));
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
        {
            var result = _service.GetOperationHistory(operationType)
                .ConvertAll(dto => ToApiDto(dto));

            return Ok(result);
        }

        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
        {
            var result = _service.GetMeasurementsByType(measurementType)
                .ConvertAll(dto => ToApiDto(dto));

            return Ok(result);
        }

        [Authorize]
        [HttpGet("history")]
        public IActionResult GetHistoryByAuthenticatedUser()
        {
            int userId = GetCurrentUserId();
            var result = _service.GetHistoryByUserId(userId)
                .ConvertAll(dto => ToApiDto(dto));

            return Ok(result);
        }

        [Authorize]
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            int userId = GetCurrentUserId();
            int total = _service.GetOperationCountByUserId(userId);

            return Ok(new { totalOperations = total });
        }

        [HttpGet("count")]
        public IActionResult GetTotalOperationCount()
        {
            int userId = GetCurrentUserId();
            int total = _service.GetOperationCountByUserId(userId);

            return Ok(total);
        }

        private static BusinessQuantityDTO ToBusinessDto(QuantityDTO dto)
        {
            return new BusinessQuantityDTO(dto.Value, dto.Unit, dto.MeasurementType);
        }

        private static BusinessQuantityDTO ToBusinessDto(MathQuantityDTO dto, string measurementType)
        {
            return new BusinessQuantityDTO(dto.Value, dto.Unit!.Trim(), measurementType);
        }

        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("id")?.Value;

            if (int.TryParse(idClaim, out int userId))
            {
                return userId;
            }

            string? emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(emailClaim))
            {
                string email = emailClaim.Trim().ToLowerInvariant();
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                if (user is not null)
                {
                    return user.Id;
                }
            }

            throw new QuantityMeasurementException("Unable to resolve user identity from token.");
        }

        private static QuantityMeasurementDTO ToApiDto(BusinessQuantityMeasurementDTO dto, string? requestedTargetUnit = null)
        {
            string normalizedRequestedUnit = requestedTargetUnit?.Trim() ?? string.Empty;
            string resolvedTargetUnit = string.IsNullOrWhiteSpace(normalizedRequestedUnit)
                ? dto.ResultUnit
                : normalizedRequestedUnit;

            return new QuantityMeasurementDTO
            {
                Id = dto.Id,
                CreatedAt = dto.CreatedAt,
                ThisValue = dto.ThisValue,
                ThisUnit = dto.ThisUnit,
                ThisMeasurementType = dto.ThisMeasurementType,
                ThatValue = dto.ThatValue,
                ThatUnit = dto.ThatUnit,
                ThatMeasurementType = dto.ThatMeasurementType,
                ResultValue = ParseResultValue(dto.ResultValue),
                ResultUnit = resolvedTargetUnit,
                TargetUnit = resolvedTargetUnit,
                ResultString = dto.ResultString,
                Operation = dto.Operation,
                IsError = dto.IsError,
                ErrorMessage = dto.ErrorMessage
            };
        }

        private static decimal ParseResultValue(string resultValue)
        {
            if (string.IsNullOrWhiteSpace(resultValue))
            {
                return 0m;
            }

            string trimmed = resultValue.Trim();

            if (decimal.TryParse(resultValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(resultValue, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed))
            {
                return parsed;
            }

            string normalized = trimmed.ToLowerInvariant();
            if (normalized is "true" or "equal" or "equals")
            {
                return 1m;
            }

            if (normalized is "false" or "not equal" or "notequal")
            {
                return 0m;
            }

            string firstToken = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            if (decimal.TryParse(firstToken, NumberStyles.Float, CultureInfo.InvariantCulture, out parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(firstToken, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed))
            {
                return parsed;
            }

            Console.WriteLine($"Unable to parse ResultValue '{resultValue}'. Returning 0.");
            return 0m;
        }

        private static Dictionary<string, string[]> ValidateDivideRequest(MathRequestDTO? request)
        {
            var errors = new Dictionary<string, string[]>();

            if (request is null)
            {
                errors[nameof(MathRequestDTO)] = ["Request body is required."];
                return errors;
            }

            if (request.FirstQuantityDTO is null)
            {
                errors[nameof(MathRequestDTO.FirstQuantityDTO)] = ["FirstQuantityDTO is required."];
            }

            if (request.SecondQuantityDTO is null)
            {
                errors[nameof(MathRequestDTO.SecondQuantityDTO)] = ["SecondQuantityDTO is required."];
            }

            if (string.IsNullOrWhiteSpace(request.MeasurementType))
            {
                errors[nameof(MathRequestDTO.MeasurementType)] = ["MeasurementType is required."];
            }

            if (request.FirstQuantityDTO is not null && string.IsNullOrWhiteSpace(request.FirstQuantityDTO.Unit))
            {
                errors[$"{nameof(MathRequestDTO.FirstQuantityDTO)}.{nameof(MathQuantityDTO.Unit)}"] = ["Unit is required."];
            }

            if (request.SecondQuantityDTO is not null && string.IsNullOrWhiteSpace(request.SecondQuantityDTO.Unit))
            {
                errors[$"{nameof(MathRequestDTO.SecondQuantityDTO)}.{nameof(MathQuantityDTO.Unit)}"] = ["Unit is required."];
            }

            return errors;
        }

        private static void EnsureQuantityInput(QuantityInputDTO? input, bool requireTargetQuantity, string operationName)
        {
            if (input is null)
            {
                throw new QuantityMeasurementException("Request body is required.");
            }

            if (input.ThisQuantityDTO is null)
            {
                throw new QuantityMeasurementException("ThisQuantityDTO is required.");
            }

            if (input.ThatQuantityDTO is null)
            {
                throw new QuantityMeasurementException("ThatQuantityDTO is required.");
            }

            if (string.IsNullOrWhiteSpace(input.ThisQuantityDTO.Unit))
            {
                throw new QuantityMeasurementException("ThisQuantityDTO.Unit is required.");
            }

            if (string.IsNullOrWhiteSpace(input.ThatQuantityDTO.Unit))
            {
                throw new QuantityMeasurementException("ThatQuantityDTO.Unit is required.");
            }

            if (string.IsNullOrWhiteSpace(input.ThisQuantityDTO.MeasurementType))
            {
                throw new QuantityMeasurementException("ThisQuantityDTO.MeasurementType is required.");
            }

            if (string.IsNullOrWhiteSpace(input.ThatQuantityDTO.MeasurementType))
            {
                throw new QuantityMeasurementException("ThatQuantityDTO.MeasurementType is required.");
            }

            if (requireTargetQuantity)
            {
                if (input.TargetQuantityDTO is null)
                {
                    throw new QuantityMeasurementException($"TargetQuantityDTO is required for {operationName}.");
                }

                if (string.IsNullOrWhiteSpace(input.TargetQuantityDTO.Unit))
                {
                    throw new QuantityMeasurementException($"TargetQuantityDTO.Unit is required for {operationName}.");
                }
            }
        }
    }
}
