using FrameRent.Application.DTOs.Rentals;
using FrameRent.Application.UseCases.Rentals.ApproveRental;
using FrameRent.Application.UseCases.Rentals.CancelRental;
using FrameRent.Application.UseCases.Rentals.CreateRental;
using FrameRent.Application.UseCases.Rentals.DeliverRental;
using FrameRent.Application.UseCases.Rentals.GetAllRentals;
using FrameRent.Application.UseCases.Rentals.GetMyRentals;
using FrameRent.Application.UseCases.Rentals.GetRentalById;
using FrameRent.Application.UseCases.Rentals.RejectRental;
using FrameRent.Application.UseCases.Rentals.ReturnRental;
using FrameRent.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrameRent.Api.Controllers;

[ApiController]
[Route("api/v1/rentals")]
public class RentalsController : ControllerBase
{
    private readonly CreateRentalUseCase _createRentalUseCase;
    private readonly GetMyRentalsUseCase _getMyRentalsUseCase;
    private readonly GetRentalByIdUseCase _getRentalByIdUseCase;
    private readonly GetAllRentalsUseCase _getAllRentalsUseCase;
    private readonly CancelRentalUseCase _cancelRentalUseCase;
    private readonly ApproveRentalUseCase _approveRentalUseCase;
    private readonly RejectRentalUseCase _rejectRentalUseCase;
    private readonly DeliverRentalUseCase _deliverRentalUseCase;
    private readonly ReturnRentalUseCase _returnRentalUseCase;

    public RentalsController(
        CreateRentalUseCase createRentalUseCase,
        GetMyRentalsUseCase getMyRentalsUseCase,
        GetRentalByIdUseCase getRentalByIdUseCase,
        GetAllRentalsUseCase getAllRentalsUseCase,
        CancelRentalUseCase cancelRentalUseCase,
        ApproveRentalUseCase approveRentalUseCase,
        RejectRentalUseCase rejectRentalUseCase,
        DeliverRentalUseCase deliverRentalUseCase,
        ReturnRentalUseCase returnRentalUseCase)
    {
        _createRentalUseCase = createRentalUseCase;
        _getMyRentalsUseCase = getMyRentalsUseCase;
        _getRentalByIdUseCase = getRentalByIdUseCase;
        _getAllRentalsUseCase = getAllRentalsUseCase;
        _cancelRentalUseCase = cancelRentalUseCase;
        _approveRentalUseCase = approveRentalUseCase;
        _rejectRentalUseCase = rejectRentalUseCase;
        _deliverRentalUseCase = deliverRentalUseCase;
        _returnRentalUseCase = returnRentalUseCase;
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Usuario no autenticado.");

        return userId;
    }

    [Authorize(Roles = "Customer")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRentalRequest request)
    {
        try
        {
            var customerId = GetCurrentUserId();
            var id = await _createRentalUseCase.ExecuteAsync(customerId, request);
            return Ok(id);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("mine")]
    public async Task<ActionResult<IReadOnlyCollection<RentalResponse>>> GetMine()
    {
        var customerId = GetCurrentUserId();
        var rentals = await _getMyRentalsUseCase.ExecuteAsync(customerId);
        return Ok(rentals);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RentalResponse>> GetById(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var isStaff = User.IsInRole("Staff");

        var rental = await _getRentalByIdUseCase.ExecuteAsync(id);
        if (rental is null)
            return NotFound();

        if (!isStaff && rental.CustomerId != currentUserId)
            return Forbid();

        return Ok(rental);
    }

    [Authorize]
    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult> Cancel(Guid id)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var isStaff = User.IsInRole("Staff");
            await _cancelRentalUseCase.ExecuteAsync(id, currentUserId, isStaff);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Staff")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<RentalResponse>>> GetAll([FromQuery] RentalStatus? status = null)
    {
        var rentals = await _getAllRentalsUseCase.ExecuteAsync(status);
        return Ok(rentals);
    }

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id:guid}/approve")]
    public async Task<ActionResult> Approve(Guid id)
    {
        try
        {
            await _approveRentalUseCase.ExecuteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id:guid}/reject")]
    public async Task<ActionResult> Reject(Guid id)
    {
        try
        {
            await _rejectRentalUseCase.ExecuteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id:guid}/deliver")]
    public async Task<ActionResult> Deliver(Guid id)
    {
        try
        {
            await _deliverRentalUseCase.ExecuteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Staff")]
    [HttpPatch("{id:guid}/return")]
    public async Task<ActionResult> Return(Guid id, [FromQuery] DateTime actualReturnDate)
    {
        try
        {
            await _returnRentalUseCase.ExecuteAsync(id, actualReturnDate);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
