using Contracts.DTOs.RestaurantDTO;
using Domain.Entities.RestaurantModule;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAbstraction
{
    public interface IReservationService
    {
        Task<Result<CreateReservationResponseDto>> CreateReservationAsync(
            CreateReservationDto dto,
            CancellationToken cancellationToken = default);

        Task<Result<ReservationDetailsDto>> GetReservationByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<Result<IEnumerable<ReservationDetailsDto>>> GetAllReservationsAsync(
            CancellationToken cancellationToken = default);

        Task<Result<bool>> UpdateReservationAsync(
            int id,
            UpdateReservationDto dto,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> DeleteReservationAsync(
            int id,
            CancellationToken cancellationToken = default);

    }
}
