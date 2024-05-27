using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private PraDrivingSchoolContext _context;

        public ReservationsController(PraDrivingSchoolContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public ActionResult<IEnumerable<ReservationDto>> Get()
        {
            try
            {
                var reservations = _context.Rezervations;

                if (reservations.IsNullOrEmpty())
                {
                    return StatusCode(404, "No reservations found.");
                }

                var reservationsDto = reservations.Select(reservation => new ReservationDto
                {
                    Id = reservation.Idrezervation,
                    StudentId = reservation.StudentId,
                    InstructorId = reservation.InstructorId,
                    StateId = reservation.StateId,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                });

                return Ok(reservationsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //// GET api/reservations/id
        [HttpGet("{id}")]
        public ActionResult<ReservationDto> Get(int id)
        {
            try
            {
                var reservation = _context.Rezervations.FirstOrDefault(reservation => reservation.Idrezervation == id);

                if (reservation == null)
                {
                    return StatusCode(404, $"No reservations found with Id: {id}.");
                }

                var reservationDto =  new ReservationDto
                {
                    Id = reservation.Idrezervation,
                    StudentId = reservation.StudentId,
                    InstructorId = reservation.InstructorId,
                    StateId = reservation.StateId,
                    StartDate = reservation.StartDate,
                    EndDate = reservation.EndDate,
                };

                return Ok(reservationDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        //// UPDATE api/reservations/id
        [HttpPost]
        public ActionResult AddReservation([FromBody] ReservationDto reservationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var reservation = new Rezervation
                {
                    StudentId = reservationDto.StudentId,
                    InstructorId = reservationDto.InstructorId,
                    StateId = reservationDto.StateId,
                    StartDate = reservationDto.StartDate,
                    EndDate = reservationDto.EndDate,
                };

                _context.Add(reservation);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateReservation([FromRoute] int id, [FromBody] ReservationDto reservationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var reservation = _context.Rezervations.FirstOrDefault(r => r.Idrezervation == id);
                if (reservation == null)
                {
                    return NotFound($"Could not find reservation with id {id}");
                }

                reservation.StudentId = reservationDto.StudentId;
                reservation.InstructorId = reservationDto.InstructorId;
                reservation.StateId = reservationDto.StateId;
                reservation.StartDate = reservationDto.StartDate;
                reservation.EndDate = reservationDto.EndDate;

                _context.SaveChanges();

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //// api/reservations/id
        [HttpDelete("{id}")]
        public ActionResult DeleteReservation(int id)
        {
            try
            {
                var reservation = _context.Rezervations.FirstOrDefault(reservation => reservation.Idrezervation == id);

                if (reservation == null)
                {
                    return StatusCode(404, $"No reservations found with Id: {id}.");
                }

                _context.Rezervations.Remove(reservation);
                _context.SaveChanges(true);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
