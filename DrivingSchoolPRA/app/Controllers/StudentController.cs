using app.DTOs;
using app.Models;
using app.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private PraDrivingSchoolContext _context;
        private IConfiguration _configuration;

        public StudentController(PraDrivingSchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> Get()
        {
            try
            {
                if (_context.Students.Count() <= 0)
                {
                    return StatusCode(404, "No avaliable students.");
                }

                var students = _context.Students.Include(s => s.Person).Select(s => new StudentDto
                {
                    Id = s.PersonId,
                    FirstName = s.Person.FirstName,
                    Lastname = s.Person.Lastname,
                    Email = s.Person.Email,
                    HoursDriven = s.HoursDriven,
                    Oib = s.Oib
                });

                return Ok(students);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public ActionResult<StudentDto> Get(int id)
        {
            try
            {
                if (_context.Students.FirstOrDefault(s => s.PersonId == id) == null)
                {
                    return StatusCode(404, "No avaliable students.");
                }

                var s = _context.Students.Include(s => s.Person).First(s => s.PersonId == id);
                var studentDto = new StudentDto
                {
                    Id = s.PersonId,
                    FirstName = s.Person.FirstName,
                    Lastname = s.Person.Lastname,
                    Email = s.Person.Email,
                    HoursDriven = s.HoursDriven,
                    Oib = s.Oib
                };

                return Ok(studentDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<StudentDto> Register(StudentDto studentDto)
        {
            try
            {
                var email = studentDto.Email.Trim();
                if (_context.Students.Any(x => x.Person.Email.Equals(email)))
                {
                    return BadRequest($"User with email {email} already exists.");
                }

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(studentDto.Password, b64salt);

                var person = new Person
                {
                    FirstName = studentDto.FirstName,
                    Lastname = studentDto.Lastname,
                    Email = email,
                    PswdHash = b64hash,
                    PswSalt = b64salt
                };
                _context.People.Add(person);
                _context.SaveChanges();

                int id = _context.People.First(p => p.Email.Equals(email)).Idperson;
                var student = new Student
                {
                    Oib = studentDto.Oib,
                    HoursDriven = studentDto.HoursDriven,
                    PersonId = id
                };
                _context.Students.Add(student);
                _context.SaveChanges();

                studentDto.Id = student.PersonId;

                return Ok(studentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(StudentLoginDto studentDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                var existingStudent = _context.Students.Include(s => s.Person).FirstOrDefault(x => x.Person.Email == studentDto.Email);
                if (existingStudent == null)
                {
                    return BadRequest(genericLoginFail);
                }

                var b64hash = PasswordHashProvider.GetHash(studentDto.Password, existingStudent.Person.PswSalt);
                if (b64hash != existingStudent.Person.PswdHash)
                {
                    return BadRequest(genericLoginFail);
                }

                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, studentDto.Email, "Student");

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_context.Students.FirstOrDefault(s => s.PersonId == id) == null)
                {
                    return BadRequest("Can't delete non existing student.");
                }

                var student = _context.Students.FirstOrDefault(s => s.PersonId == id);
                var person = _context.People.FirstOrDefault(p => p.Idperson == id);

                _context.Students.Remove(student);
                _context.People.Remove(person);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
