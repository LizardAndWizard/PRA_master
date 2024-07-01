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

                var students = _context.Students
                    .Include(s => s.Person)
                    .Include(s => s.Vehicle)
                    .Include(s => s.Vehicle.Model)
                    .Include(s => s.Vehicle.Model.Brand)
                    .Include(s => s.Vehicle.Colour)
                    .Include(s => s.Vehicle.Category)
                    .Select(s => MapStudentToDto(s));

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

                var s = _context.Students
                    .Include(s => s.Person)
                    .Include(s => s.Vehicle)
                    .Include(s => s.Vehicle.Model)
                    .Include(s => s.Vehicle.Model.Brand)
                    .Include(s => s.Vehicle.Colour)
                    .Include(s => s.Vehicle.Category)
                    .First(s => s.PersonId == id);
                var studentDto = MapStudentToDto(s);

                return Ok(studentDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private static StudentDto MapStudentToDto(Student student)
        {
            var studentDto = new StudentDto 
            {
                Id = student.PersonId,
                FirstName = student.Person.FirstName,
                Lastname = student.Person.Lastname,
                Email = student.Person.Email,
                HoursDriven = student.HoursDriven,
                Oib = student.Oib,
                InstructorId = student.InstructorId
            };

            if (student.VehicleId != null)
            {
                studentDto.VehicleId = student.VehicleId;
                studentDto.Vehicle = new VehicleDto
                {
                    Idvehicle = student.Vehicle.Idvehicle,
                    Model = student.Vehicle.Model.Name,
                    Brand = student.Vehicle.Model.Brand.Name,
                    Category = student.Vehicle.Category.Name,
                    Colour = student.Vehicle.Colour.Name,
                    Picture = null //vehicle.Picture
                };
            }

            return studentDto;
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
                    PersonId = id,
                    VehicleId = studentDto.VehicleId
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
        public ActionResult<LoginReturnDto> Login(LoginDto studentDto)
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

                var returnDto = new LoginReturnDto 
                { 
                    IdPerson = existingStudent.PersonId, 
                    Token = serializedToken,
                    OIB = existingStudent.Oib
                };

                return Ok(returnDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<StudentDto> UpdateStudent(int id, [FromBody] UpdateStudentDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Student does not contain valid data");
                }

                var student = _context.Students.FirstOrDefault(x => x.PersonId == id);

                if (studentDto.InstructorId != 0)
                {
                    student.InstructorId = studentDto.InstructorId;
                }

                if (studentDto.VehicleId != 0)
                {
                    student.VehicleId = studentDto.VehicleId;
                }

                if (studentDto.HoursDriven != 0)
                {
                    student.HoursDriven = studentDto.HoursDriven;
                }

                _context.SaveChanges();

                return Ok(studentDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
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
