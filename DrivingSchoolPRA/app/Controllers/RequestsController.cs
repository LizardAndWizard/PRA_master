using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PraDrivingSchoolContext _context;

        public RequestsController(PraDrivingSchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RequestDto>> Get()
        {
            try
            {
                var requests = _context.Requests;

                if (requests.IsNullOrEmpty())
                {
                    return StatusCode(404, "No requests found.");
                }

                var requestsDto = requests.Select(request => new RequestDto
                {
                    Idrequest = request.Idrequest,
                    StudentId = request.StudentId,
                    InstructorId = request.InstructorId, 
                    VehicleId = request.VehicleId,
                    StateId = request.StateId,
                });

                return Ok(requestsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("instructor/{id}")]
        public ActionResult<IEnumerable<RequestDto>> GetForInstructor(int id)
        {
            try
            {
                var requests = _context.Requests.Where(r => r.InstructorId == id);

                if (requests.IsNullOrEmpty())
                {
                    return StatusCode(404, "No requests found.");
                }

                var requestsDto = requests.Select(request => new RequestDto
                {
                    Idrequest = request.Idrequest,
                    StudentId = request.StudentId,
                    InstructorId = request.InstructorId,
                    VehicleId = request.VehicleId,
                    StateId = request.StateId,
                });

                return Ok(requestsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult AddRequest([FromBody] RequestDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var request = new Request
                {
                    StudentId = requestDto.StudentId,
                    InstructorId = requestDto.InstructorId,
                    StateId = requestDto.StateId,
                    VehicleId = requestDto.VehicleId
                };

                _context.Add(request);
                _context.SaveChanges();

                return Ok(requestDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<RequestDto> UpdateRequest(int id, [FromBody] RequestDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var request = _context.Requests.FirstOrDefault(x => x.Idrequest == id);

                request.StateId = requestDto.StateId;

                _context.SaveChanges();

                return Ok(requestDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public ActionResult<RequestDto> DeleteRequest(int id)
        {
            try
            {
                var request = _context.Requests.FirstOrDefault(x => x.Idrequest == id);
                _context.Requests.Remove(request);
                _context.SaveChanges();

                return Ok(request);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
