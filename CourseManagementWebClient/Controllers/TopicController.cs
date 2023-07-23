using BussinessObject.DTOs;
using BussinessObject.Models;
using CourseManagementWebClientWebClient.Controllers;
using CourseManagementWebClientWebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourseManagementWebClient.Controllers
{
    public class TopicController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient client;
        private string TopicApiUrl = "";

        public TopicController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            TopicApiUrl = "https://localhost:7167/api/topics";
        }

        [Authorize(Roles = "TEACHER")]
        public IActionResult Create(int id)
        {
            return View();
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TopicDTO topicDTO, int id)
        {
            using (var httpClient = new HttpClient())
            {
                topicDTO.CourseId = id;
                using (var response = await httpClient.PostAsJsonAsync(TopicApiUrl, topicDTO))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Redirect("/Course/Details/" + id);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet("Topic/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var topic = await GetTopicById(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] TopicDTO topicDTO, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(topicDTO);
            }
            var topic = await GetTopicById(id);
            topicDTO.CourseId = topic.CourseId;
            HttpResponseMessage response = await client.PutAsJsonAsync(TopicApiUrl, topicDTO);
            return Redirect("/Course/Details/" + topicDTO.CourseId);
        }

        [Authorize(Roles = "TEACHER,STUDENT")]
        public async Task<IActionResult> Details(int id)
        {
            var topic = await GetTopicById(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        private async Task<TopicDTO?> GetTopicById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            HttpResponseMessage response = await client.GetAsync(TopicApiUrl + "/" + id + "/topic");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content.ReadFromJsonAsync<TopicDTO>().Result;
            }
            return null;
        }

        [Authorize(Roles = "TEACHER")]
        public async Task<IActionResult> Delete(int? id)
        {
            var topic = await GetTopicById(id);
            if (topic == null)
            {
                return NotFound();
            }
            await client.DeleteAsync(TopicApiUrl + "/id?id=" + id);
            return Redirect("/Course/Details/" + topic.CourseId);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet]
        public IActionResult UploadMaterial(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        public async Task<IActionResult> UploadMaterial(IFormFile file, int id)
        {
            // Create a new FormData instance and add the file to it
            MultipartFormDataContent formData = new MultipartFormDataContent();
            var topic = await GetTopicById(id);
            //Dang loi o day
            StreamContent fileContent = new StreamContent(file.OpenReadStream());
            formData.Add(fileContent, "file", file.FileName);

            try
            {
                // Send a POST request to the API endpoint with the file
                HttpResponseMessage response = await client.PostAsync(TopicApiUrl + "/upload-meterial/" + id, formData);

                // Check if the response was successful
                if (response.IsSuccessStatusCode)
                {
                    // File uploaded successfully
                    ViewData["Message"] = "File uploaded successfully";
                }
                else
                {
                    // File upload failed
                    ViewData["Message"] = "File upload failed";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request
                ViewData["Message"] = "An error occurred: " + ex.Message;
            }
            // Return the view with the message
            return Redirect("/Course/Details/" + topic.CourseId);
        }

        [Authorize(Roles = "TEACHER,STUDENT")]
        [HttpGet("Topic/Dowload/{id}")]
        public async Task<IActionResult> DowloadMaterial(int id)
        {
            var apiUrl = TopicApiUrl + "/dowload-meterial/" + id;
            var topic = await GetTopicById(id);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var fileBytes = await response.Content.ReadAsByteArrayAsync();
                    return File(fileBytes, "application/octet-stream", topic.MaterialDTO.Name);
                }
                else
                {
                    return Content("Error downloading file.");
                }
            }
        }
    }
}
