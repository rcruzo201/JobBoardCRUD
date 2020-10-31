using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    /// <summary>
    /// Job Controller conining the CRUD funtions
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IList<DTOJob> _jobList = new List<DTOJob>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="webHostEnvironment"></param>
        public JobController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// List all the jobs in the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetJobs()
        {
            //var jobs = _unitOfWork.Jobs.GetOrderedJobs();
            ReadFiles();

            return Ok(_jobList);
        }

        /// <summary>
        /// This will create a Job Object
        /// </summary>
        /// <param name="jobObject"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateJob([FromBody] Job jobObject)
        {
            var jobMapping = new DTOJob
            {
                JobId = jobObject.JobId == -1 ? LongRandom() : jobObject.JobId,
                JobTitle = jobObject.JobTitle,
                Description = jobObject.Description,
                CreatedAt = jobObject.CreatedAt,
                ExpiresAt = jobObject.ExpiresAt
            };

            CreateFile(jobMapping);
            //_unitOfWork.Jobs.Add(jobMapping);
            //_unitOfWork.Complete();
            return Ok();
        }
        private long LongRandom()
        {
            Random r = new Random();

            return (long)((r.NextDouble() * 2.0 - 1.0) * long.MaxValue);
        }
        /// <summary>
        /// Method used for Generating Json Files to Store Data
        /// </summary>
        /// <param name="obj"></param>
        private void CreateFile(DTOJob obj)
        {
            string uploadDir = Path.Combine(_webHostEnvironment.ContentRootPath, "Json");
            var fileName = Guid.NewGuid().ToString() + ".json";
            //use the guid for file identification when updating
            obj.Guid = fileName;
            JObject jObject = (JObject)JToken.FromObject(obj);
            string filePath = Path.Combine(uploadDir, fileName);
            System.IO.File.WriteAllText(filePath, jObject.ToString());
        }
        private void DeleteFile(DTOJob obj)
        {
            JObject jObject = (JObject)JToken.FromObject(obj);
            string uploadDir = Path.Combine(_webHostEnvironment.ContentRootPath, "Json");
            string filePath = Path.Combine(uploadDir, obj.Guid);
            System.IO.File.Delete(filePath);
        }
        /// <summary>
        /// Method that read a list of files in a directory and convert them to a serialize Json object
        /// </summary>
        private void ReadFiles()
        {
            _jobList = new List<DTOJob>();

            string uploadDir = Path.Combine(_webHostEnvironment.ContentRootPath, "Json");
            foreach (var file in Directory.GetFiles(uploadDir, "*.json"))
            {
                using (StreamReader r = new StreamReader(file))
                {
                    string json = r.ReadToEnd();
                    _jobList.Add(JsonConvert.DeserializeObject<DTOJob>(json));
                }
            }
        }
        /// <summary>
        /// Update the selected Job
        /// </summary>
        /// <param name="jobObject"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateJob([FromBody] DTOJob jobObject)
        {
            //_unitOfWork.Jobs.Update(jobObject);
            //_unitOfWork.Complete();
            DeleteJob(jobObject);
            CreateJob(jobObject);

            return Ok();
        }

        /// <summary>
        /// Delete any object from the DB
        /// </summary>
        /// <param name="jobObject"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteJob([FromBody] DTOJob jobObject)
        {
            //_unitOfWork.Jobs.Remove(jobObject);
            //_unitOfWork.Complete();
            DeleteFile(jobObject);
            return Ok();

        }
    }

    
}
