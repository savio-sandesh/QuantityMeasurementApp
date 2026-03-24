using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementWebApi.Models;
using BusinessQuantityDTO = ModelLayer.QuantityDTO;
using BusinessQuantityMeasurementDTO = ModelLayer.QuantityMeasurementDTO;

namespace QuantityMeasurementWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        [HttpPost("compare")]
        public IActionResult Compare([FromBody] QuantityInputDTO input)
        {
            var result = _service.Compare(ToBusinessDto(input.ThisQuantityDTO), ToBusinessDto(input.ThatQuantityDTO));
            return Ok(ToApiDto(result));
        }

        [HttpPost("convert")]
        public IActionResult Convert([FromBody] QuantityInputDTO input)
        {
            if (input.TargetQuantityDTO is null || string.IsNullOrWhiteSpace(input.TargetQuantityDTO.Unit))
            {
                throw new QuantityMeasurementException("TargetQuantityDTO.Unit is required for conversion.");
            }

            var result = _service.Convert(ToBusinessDto(input.ThisQuantityDTO), input.TargetQuantityDTO.Unit);
            return Ok(ToApiDto(result));
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] QuantityInputDTO input)
        {
            if (input.TargetQuantityDTO is null || string.IsNullOrWhiteSpace(input.TargetQuantityDTO.Unit))
            {
                throw new QuantityMeasurementException("TargetQuantityDTO.Unit is required for addition.");
            }

            var result = _service.Add(
                ToBusinessDto(input.ThisQuantityDTO),
                ToBusinessDto(input.ThatQuantityDTO),
                input.TargetQuantityDTO.Unit);

            return Ok(ToApiDto(result));
        }

        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] QuantityInputDTO input)
        {
            if (input.TargetQuantityDTO is null || string.IsNullOrWhiteSpace(input.TargetQuantityDTO.Unit))
            {
                throw new QuantityMeasurementException("TargetQuantityDTO.Unit is required for subtraction.");
            }

            var result = _service.Subtract(
                ToBusinessDto(input.ThisQuantityDTO),
                ToBusinessDto(input.ThatQuantityDTO),
                input.TargetQuantityDTO.Unit);

            return Ok(ToApiDto(result));
        }

        [HttpPost("divide")]
        public IActionResult Divide([FromBody] QuantityInputDTO input)
        {
            var result = _service.Divide(ToBusinessDto(input.ThisQuantityDTO), ToBusinessDto(input.ThatQuantityDTO));
            return Ok(ToApiDto(result));
        }

        [HttpGet("history/operation/{operationType}")]
        public IActionResult GetHistoryByOperation(string operationType)
        {
            var result = _service.GetOperationHistory(operationType)
                .ConvertAll(ToApiDto);

            return Ok(result);
        }

        [HttpGet("history/type/{measurementType}")]
        public IActionResult GetHistoryByType(string measurementType)
        {
            var result = _service.GetMeasurementsByType(measurementType)
                .ConvertAll(ToApiDto);

            return Ok(result);
        }

        [HttpGet("count")]
        public IActionResult GetTotalOperationCount()
        {
            int total =
                _service.GetOperationCount(OperationTypeConstants.Compare) +
                _service.GetOperationCount(OperationTypeConstants.Add) +
                _service.GetOperationCount(OperationTypeConstants.Subtract) +
                _service.GetOperationCount(OperationTypeConstants.Divide) +
                _service.GetOperationCount(OperationTypeConstants.Convert);

            return Ok(total);
        }

        private static BusinessQuantityDTO ToBusinessDto(QuantityDTO dto)
        {
            return new BusinessQuantityDTO(dto.Value, dto.Unit, dto.MeasurementType);
        }

        private static QuantityMeasurementDTO ToApiDto(BusinessQuantityMeasurementDTO dto)
        {
            return new QuantityMeasurementDTO
            {
                ThisValue = dto.ThisValue,
                ThisUnit = dto.ThisUnit,
                ThisMeasurementType = dto.ThisMeasurementType,
                ThatValue = dto.ThatValue,
                ThatUnit = dto.ThatUnit,
                ThatMeasurementType = dto.ThatMeasurementType,
                ResultValue = dto.ResultValue,
                ResultUnit = dto.ResultUnit,
                ResultString = dto.ResultString,
                Operation = dto.Operation,
                IsError = dto.IsError,
                ErrorMessage = dto.ErrorMessage
            };
        }
    }
}
