using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Common;
using CollegeApp.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StduentController : ControllerBase
    {
        private readonly ILogger<StduentController> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;
        public StduentController(ILogger<StduentController> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StudentDTO))]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudens()
        {
            _logger.LogInformation("Get Students Method Start");
            var students = await _studentRepository.GetAllAsync();
            var stdDTOData = _mapper.Map<List<StudentDTO>>(students);
            return Ok(stdDTOData);
        }
        [HttpGet()]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StudentDTO))]
        public async Task<ActionResult<StudentDTO>> GetStudenById(int id)
        {
            _logger.LogInformation("GetStudenById Method Start");
            if (id <= 0)
            {
                _logger.LogError("GetStudenById Method badrequest");
                return BadRequest();
            }
            var student = await _studentRepository.GetByIdAsync(student => student.Id == id, false);
            if (student == null)
            {
                _logger.LogWarning("GetStudenById Method NotFound");
                return NotFound($"This student with id {id}, Not Found");
            }
            var stddto = _mapper.Map<StudentDTO>(student);
            return Ok(stddto);
        }
        [HttpGet("{studentname:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StudentDTO))]
        public async Task<ActionResult> GetStudenByName(string studentname)
        {
            if (studentname == string.Empty)
            {
                return BadRequest();
            }
            var student = await _studentRepository.GetByNameAsync(student => student.StdName.ToLower() == studentname.ToLower());
            if (student == null)
            {
                return NotFound($"This student with name {studentname}, Not Found");
            }
            var stddto = _mapper.Map<StudentDTO>(student);
            return Ok(stddto);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(StudentDTO))]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (studentDTO == null)
                return BadRequest();
            //if (model.DateofAddmision < DateTime.Now)
            //{
            //    ModelState.AddModelError("AddmissionDate Error", "Pls Enter Today's Date.");
            //    return BadRequest(ModelState);
            //}
            Student student = _mapper.Map<Student>(studentDTO);
            var newstd = await _studentRepository.CreateAsyn(student);
            return CreatedAtRoute("GetStudentById", new { id = newstd.Id }, newstd);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(StudentDTO))]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
                return BadRequest();
            var existingstudent = await _studentRepository.GetByIdAsync(student => student.Id == studentDTO.Id, true);
            if (existingstudent == null)
            {
                return NotFound();
            }
            Student student = _mapper.Map<Student>(studentDTO);
            await _studentRepository.UpdateAsync(student);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StudentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(StudentDTO))]
        public async Task<ActionResult<StudentDTO>> UpdatePatialStudentData(int id, [FromBody] JsonPatchDocument<StudentDTO> jsonPatchDocument)
        {
            if (id < 0)
                return NotFound();
            var existingstudent = await _studentRepository.GetByIdAsync(student => student.Id == id, true);
            var stddto = _mapper.Map<StudentDTO>(existingstudent);
            jsonPatchDocument.ApplyTo(stddto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var std = _mapper.Map<Student>(stddto);
            await _studentRepository.UpdatePartialAsync(std);
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(bool))]
        public async Task<ActionResult> DeleteStudenById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Student student = await _studentRepository.GetByIdAsync(student => student.Id == id, false);
            if (student == null)
            {
                return NotFound($"This student with id {id}, Not Found");
            }
            await _studentRepository.DeleteAsyn(student);
            return Ok(true);
        }
    }
}
