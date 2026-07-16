using Contracts.DTOs.RestaurantDTO;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;

namespace Presentation.Controllers.Restaurant
{
    public class ReservationsController : ApiBaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        #region Create

        [HttpPost("create")]
        public async Task<ActionResult<CreateReservationResponseDto>> CreateReservation(
            [FromBody] CreateReservationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _reservationService.CreateReservationAsync(dto, cancellationToken);
            return HandleResult(result);
        }

        #endregion

        #region Read

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReservationDetailsDto>> GetReservationById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var result = await _reservationService.GetReservationByIdAsync(id, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDetailsDto>>> GetAllReservations(
            CancellationToken cancellationToken)
        {
            var result = await _reservationService.GetAllReservationsAsync(cancellationToken);
            return HandleResult(result);
        }

        #endregion

        #region Update
        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> UpdateReservation(
            [FromRoute] int id,
            [FromBody] UpdateReservationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _reservationService.UpdateReservationAsync(id, dto, cancellationToken);
            return HandleResult(result);
        }
        #endregion

        #region Delete

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteReservation(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var result = await _reservationService.DeleteReservationAsync(id, cancellationToken);
            return HandleResult(result);
        }

        #endregion

    }
}
