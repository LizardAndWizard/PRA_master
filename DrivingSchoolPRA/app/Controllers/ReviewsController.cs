using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet]
        public ActionResult<IEnumerable<ReviewDto>> Get()
        {
            try
            {
                var reviews = _context.Reviews;

                if (reviews.IsNullOrEmpty())
                {
                    return StatusCode(404, "No reviews found.");
                }

                var reviewsDto = reviews.Select(review => new ReviewDto
                {
                    Id = review.Idreview,
                    StudentOIB = review.StudentId,
                    InstructorId = review.InstructorId,
                    Grade = review.Grade,
                    Comment = review.Comment,

                });

                return Ok(reviewsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
