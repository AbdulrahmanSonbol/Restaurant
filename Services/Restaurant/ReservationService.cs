using AutoMapper;
using Contracts.DTOs.RestaurantDTO;
using Domain.Entities.RestaurantModule;
using ServiceAbstraction;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Restaurant
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Create

        public async Task<Result<CreateReservationResponseDto>> CreateReservationAsync(CreateReservationDto dto, CancellationToken cancellationToken = default)
        {
            var reservation = _mapper.Map<Reservation>(dto);
            var repository = _unitOfWork.GetRepository<Reservation, int>();

            await repository.AddAsync(reservation, cancellationToken);
            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (rowsAffected <= 0)
            {
                return Result<CreateReservationResponseDto>.Fail(
                    Error.Failure("Reservation.SaveFailed", "Failed to save the reservation."));
            }

            var responseDto = new CreateReservationResponseDto
            {
                ReservationId = reservation.Id,
                ReservationNumber = reservation.ReservationNumber
            };

            return Result<CreateReservationResponseDto>.Ok(responseDto);
        }

        #endregion

        #region Read

        #region By ID

        public async Task<Result<ReservationDetailsDto>> GetReservationByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<Reservation, int>();
            var reservation = await repository.GetByIdAsync(id, cancellationToken);

            if (reservation is null)
            {
                return Result<ReservationDetailsDto>.Fail(
                    Error.NotFound("Reservation.NotFound", $"Reservation with ID {id} was not found."));
            }

            var responseDto = _mapper.Map<ReservationDetailsDto>(reservation);

            return Result<ReservationDetailsDto>.Ok(responseDto);
        } 

        #endregion

        #region Get All

        public async Task<Result<IEnumerable<ReservationDetailsDto>>> GetAllReservationsAsync(CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<Reservation, int>();
            var reservations = await repository.GetAllAsync(cancellationToken);

            var responseDtos = _mapper.Map<IEnumerable<ReservationDetailsDto>>(reservations);
            return Result<IEnumerable<ReservationDetailsDto>>.Ok(responseDtos);
        }

        #endregion

        #endregion

        #region Update

        public async Task<Result<bool>> UpdateReservationAsync(int id, UpdateReservationDto dto, CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<Reservation, int>();
            var reservation = await repository.GetByIdAsync(id, cancellationToken);

            if (reservation == null)
            {
                return Result<bool>.Fail(Error.NotFound("Reservation.NotFound", $"Reservation with ID {id} was not found."));
            }

            _mapper.Map(dto, reservation);

            repository.Update(reservation);
            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (rowsAffected <= 0)
            {
                return Result<bool>.Fail(Error.Failure("Reservation.UpdateFailed", "No changes were saved to the database."));
            }

            return Result<bool>.Ok(true);
        }

        #endregion

        #region Delete

        public async Task<Result<bool>> DeleteReservationAsync(int id, CancellationToken cancellationToken = default)
        {
            var repository = _unitOfWork.GetRepository<Reservation, int>();
            var reservation = await repository.GetByIdAsync(id, cancellationToken);

            if (reservation == null)
            {
                return Result<bool>.Fail(Error.NotFound("Reservation.NotFound", $"Reservation with ID {id} was not found."));
            }

            repository.Delete(reservation);
            var rowsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (rowsAffected <= 0)
            {
                return Result<bool>.Fail(Error.Failure("Reservation.DeleteFailed", "Failed to delete the reservation from database."));
            }

            return Result<bool>.Ok(true);
        }

        #endregion
    }
}
