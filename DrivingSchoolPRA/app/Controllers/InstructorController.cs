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
                    .Select(i => new InstructorDto
                {
                    Id = i.PersonId,
                    FirstName = i.Person.FirstName,
                    Lastname = i.Person.Lastname,
                    Email = i.Person.Email,
                    Vehicle = null, //i.Vehicle,
                    Rating = CalculateRating(i.Idinstructor)
                });

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
                    .First(i => i.PersonId == id);

                var instructorDto = new InstructorDto
                {
                    Id = instructor.PersonId,
                    FirstName = instructor.Person.FirstName,
                    Lastname = instructor.Person.Lastname,
                    Email = instructor.Person.Email,
                    Vehicle = instructor.Vehicle,
                    Rating = CalculateRating(instructor.Idinstructor)
                };

                return Ok(instructorDto);
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
                    .Where(i => i.Person.FirstName.StartsWith(filter))
                    .Select(i => new InstructorDto
                    {
                        Id = i.PersonId,
                        FirstName = i.Person.FirstName,
                        Lastname = i.Person.Lastname,
                        Email = i.Person.Email,
                        Vehicle = i.Vehicle,
                        Rating = CalculateRating(i.Idinstructor)
                    });

                return Ok(instructors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<InstructorDto>> GetByRating(float min, float max)
        {
            try
            {
                if (min < 1 || max > 5)
                {
                    return BadRequest("Rating must be in range 1 - 5.");
                }

                var instructors = _context.Instructors
                    .Include(i => i.Person)
                    .Include(i => i.Vehicle)
                    .Select(i => new InstructorDto
                    {
                        Id = i.PersonId,
                        FirstName = i.Person.FirstName,
                        Lastname = i.Person.Lastname,
                        Email = i.Person.Email,
                        Vehicle = i.Vehicle,
                        Rating = CalculateRating(i.Idinstructor)
                    })
                    .Where(i => i.Rating >= min);

                

                return Ok(instructors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private static float CalculateRating(int id)
        {
            //return (float)_context.Instructors
            //    .First(i => i.Idinstructor == id)
            //    .Reviews
            //    .Average(r => r.Grade);
            return 5;
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


    }
}
