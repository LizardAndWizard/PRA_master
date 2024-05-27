using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private PraDrivingSchoolContext _context;

        public ReviewController(PraDrivingSchoolContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Add([FromBody]ReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var review = new Review
                {
                    StudentId = reviewDto.StudentOIB,
                    InstructorId = reviewDto.InstructorId,
                    Grade = reviewDto.Grade,
                    Comment = reviewDto.Comment,
                };

                _context.Add(review);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
