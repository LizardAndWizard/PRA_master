using app.DTOs;
using app.Models;
using app.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private PraDrivingSchoolContext _context;
        private IConfiguration _configuration;

        public InstructorController(PraDrivingSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InstructorDto>> Get()
        {
            try
            {
                if (_context.Instructors.Count() <= 0)
                {
                    return StatusCode(404, "No avaliable instructors.");
                }

                var instructors = _context.Instructors
                    .Include(i => i.Person)
                    .Include(i => i.Vehicle)
                    .Include(i => i.Vehicle.Category)
                    .Include(i => i.Vehicle.Colour)
                    .Include(i => i.Vehicle.Model)
                    .Include(i => i.Vehicle.Model.Brand)
                    .Select(i => MapInstructorToDto(i, _context));
                
                return Ok(instructors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<InstructorDto> Get(int id)
        {
            try
            {
                if (_context.Instructors.FirstOrDefault(s => s.PersonId == id) == null)
                {
                    return StatusCode(404, "No avaliable instructors.");
                }

                var instructor = _context.Instructors
                    .Include(i => i.Person)
                    .Include(i => i.Vehicle)
                    .Include(i => i.Vehicle.Category)
                    .Include(i => i.Vehicle.Colour)
                    .Include(i => i.Vehicle.Model)
                    .Include(i => i.Vehicle.Model.Brand)
                    .First(i => i.PersonId == id);

                return Ok(MapInstructorToDto(instructor, _context));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<InstructorDto>> GetByName(string filter)
        {
            try
            {
                if (!_context.Instructors.Any(i => i.Person.FirstName.StartsWith(filter)))
                {
                    return StatusCode(404, "No avaliable instructors.");
                }

                var instructors = _context.Instructors
                    .Include(i => i.Person)
                    .Include(i => i.Vehicle)
                    .Include(i => i.Vehicle.Category)
                    .Include(i => i.Vehicle.Colour)
                    .Include(i => i.Vehicle.Model)
                    .Include(i => i.Vehicle.Model.Brand)
                    .Where(i => i.Person.FirstName.StartsWith(filter))
                    .Select(i => MapInstructorToDto(i, _context));

                return Ok(instructors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<InstructorDto>> GetByRating(float min = 1, float max = 5)
        {
            try
            {
                if (min < 1 || min > 5 || max < 1 || max > 5)
                {
                    return BadRequest("Rating must be in range 1 - 5.");
                }

                List<InstructorDto> instructors = _context.Instructors
                    .Include(i => i.Person)
                    .Include(i => i.Vehicle)
                    .Include(i => i.Vehicle.Category)
                    .Include(i => i.Vehicle.Colour)
                    .Include(i => i.Vehicle.Model)
                    .Include(i => i.Vehicle.Model.Brand)
                    .Select(i => MapInstructorToDto(i, _context))
                    .ToList();

                instructors = instructors.Where(i => i.Rating >= min && i.Rating <= max).ToList();

                if (instructors.Count == 0)
                {
                    return BadRequest("No such instructors");
                }

                return Ok(instructors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<InstructorDto> Register(InstructorRegisterDto instructorDto)
        {
            try
            {
                var email = instructorDto.Email.Trim();
                if (_context.Instructors.Any(x => x.Person.Email.Equals(email)))
                {
                    return BadRequest($"User with email {email} already exists.");
                }

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(instructorDto.Password, b64salt);

                var person = new Person
                {
                    FirstName = instructorDto.FirstName,
                    Lastname = instructorDto.Lastname,
                    Email = email,
                    PswdHash = b64hash,
                    PswSalt = b64salt
                };
                _context.People.Add(person);
                _context.SaveChanges();

                int id = _context.People.First(p => p.Email.Equals(email)).Idperson;
                var instructor = new Instructor
                {
                    PersonId = id,
                    VehicleId = instructorDto.VehicleId
                };
                _context.Instructors.Add(instructor);
                _context.SaveChanges();

                instructorDto.Id = instructor.PersonId;

                return Ok(instructorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(LoginDto instructorDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingInstructor = _context.Instructors
                    .Include(s => s.Person)
                    .FirstOrDefault(x => x.Person.Email == instructorDto.Email);

                if (existingInstructor == null)
                {
                    return BadRequest(genericLoginFail);
                }

                var b64hash = PasswordHashProvider.GetHash(instructorDto.Password, existingInstructor.Person.PswSalt);
                if (b64hash != existingInstructor.Person.PswdHash)
                {
                    return BadRequest(genericLoginFail);
                }

                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, instructorDto.Email, "Instructor");

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private static float CalculateRating(int id, PraDrivingSchoolContext context)
        {
            var reviews = context.Instructors
                .Include(i => i.Reviews)
                .First(i => i.Idinstructor == id)
                .Reviews;

            if (reviews.Count() == 0)
            {
                return 0;
            }

            int sum = reviews.Sum(r => r.Grade);
            float count = reviews.Count();

            return sum / count;
        }

        private static InstructorDto MapInstructorToDto(Instructor instructor, PraDrivingSchoolContext context)
        {
            return new InstructorDto
            {
                Id = instructor.PersonId,
                FirstName = instructor.Person.FirstName,
                Lastname = instructor.Person.Lastname,
                Email = instructor.Person.Email,
                Vehicle = new VehicleDto
                {
                    Idvehicle = instructor.VehicleId,
                    Category = instructor.Vehicle.Category.Name,
                    Colour = instructor.Vehicle.Colour.Name,
                    Model = instructor.Vehicle.Model.Name,
                    Brand = instructor.Vehicle.Model.Brand.Name,
                    Picture = instructor.Vehicle.Picture
                },
                Rating = CalculateRating(instructor.Idinstructor, context)
            };
        }
    }
}
