using AutoMapper;
using BussinessObject.Context;
using BussinessObject.DTOs;
using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Repositories;

namespace CourseManagmentAPI.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicController : Controller
    {
        private ITopicRepository repository = new TopicRepository();
        private readonly IMapper mapper;

        public TopicController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        [HttpGet("id")]
        public ActionResult<IEnumerable<TopicDTO>> GetTopics(int id)
        {
            List<TopicDTO> topics = repository.getTopicsByCourseId(id).Select(mapper.Map<Topics, TopicDTO>).ToList();
            if (topics.Count == 0)
            {
                return NotFound("NOT FOUND ANY COURSE !!!");
            }
            return Ok(topics);
        }

        [HttpGet("{id}/topic")]
        public ActionResult<IEnumerable<TopicDTO>> GetTopic(int id)
        {
            Topics topic = repository.getTopic(id);
            if (topic == null)
            {
                return NotFound("TOPIC NOT FOUND !!!");
            }
            TopicDTO topicDTO = mapper.Map<TopicDTO>(topic);
            if (topic.Meterial != null)
            {
                MaterialDTO materialDTO = mapper.Map<MaterialDTO>(topic.Meterial);
                topicDTO.MaterialDTO = materialDTO;
            }
            return Ok(topicDTO);
        }
        [HttpPost]
        public IActionResult AddTopic([FromBody] TopicDTO topicDTO)
        {
            Topics topic = mapper.Map<Topics>(topicDTO);
            repository.addTopic(topic);
            return Ok("Add Successfully");
        }
        [HttpPut]
        public IActionResult UpdateTopic([FromBody] TopicDTO topicDTO)
        {
            var topic = repository.getTopic(topicDTO.Id);
            if (topicDTO == null)
            {
                return NotFound("TOPIC NOT FOUND !!!");
            }
            topic = mapper.Map<Topics>(topicDTO);
            repository.updateTopic(topic);
            return Ok("Update Successfully");
        }
        [HttpDelete("id")]
        public IActionResult DeleteTopic(int id)
        {
            var topic = repository.getTopic(id);
            if (topic == null)
            {
                return NotFound("TOPIC NOT FOUND !!!");
            }
            repository.deleteTopic(topic.Id);
            return Ok("Delete Successfully");
        }

        [HttpPost("upload-meterial/{topicId}")]
        public async Task<ActionResult> PostSingleFile(IFormFile file,int topicId)
        {
            if (file == null)
            {
                return BadRequest();
            }
            try
            {
                await repository.addMeterial(file, topicId);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("dowload-meterial/{id}")]
        public async Task<ActionResult> DownloadFile(int id)
        {
            Meterial meterial = new Meterial();
            using (var context = new MyDbContext())
            {
                meterial = context.Meterials.SingleOrDefault(x => x.Id == id);
            }
            var filepath = Path.Combine(meterial.FileRoot, meterial.FileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
    }
}

