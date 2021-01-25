using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebMaterial.BLL;
using WebMaterial.DAL.Models;
using WebMaterial.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebMaterial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public MaterialController(IMaterialService materialService, IConfiguration configuration, IFileService fileService)
        {
            _materialService = materialService;
            _configuration = configuration;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult<IList<Material>> GetAllMaterials()
        {
            return Ok(_materialService.GetAllMaterials());
        }

        // GET: api/Material/{id}
        [HttpGet("{id}")]
        public ActionResult<Material> GetMaterialsById(int id)
        {
            var material = _materialService.GetMaterialById(id);
            if (material != null)
                return Ok(material);
            return BadRequest("Не удалось найти материал с данным Id");
        }

        // GET: api/Material/{name}
        [HttpGet]
        [Route("name")]
        public IActionResult GetMaterialByName(string name)
        {
            var material = _materialService.GetMaterialByName(name);
            if (material != null)
                return Ok(material);
            return BadRequest("Не удалось найти материал с данным именем");
        }

        [HttpGet]
        [Route("filter")]
        public ActionResult<List<Material>> GetFilteredMaterials(Category category)
        {
            var materials = _materialService.GetFilteredMaterials(category);
            if (materials != null)
                return materials.ToList();
            return BadRequest("Не удалось найти материалы с данной категорией");
        }
        [HttpGet]
        [Route("download")]
        public IActionResult DownloadFile(string name, int? version)
        {
            var result = _fileService.DownloadFileFromSystem(name, version);
            if (result != null)
            {
                var material = _materialService.GetMaterialByName(name);
                return File(result, "application/octet-stream", name + '.' + material.Extension);
            }
            return BadRequest("Не удалось найти файл с данным именем");
        }

        [HttpPatch]
        public ActionResult<Material> ChangeMaterialCategory(string name, Category category)
        {
            var material = _materialService.ChangeMaterialCategory(name, category);
            if (material != null)
            {
                return Ok(material);
            }
            return BadRequest("Не удалось найти материал с данным именем");
        }

        // POST: api/Material
        [HttpPost]
        public IActionResult AddMaterial([FromForm] NewMaterialDto material, [FromForm] IFormFile file)
        {
            if (material.Name != null && file != null
                && file.Length < _configuration.GetValue<long>("Size"))
            {
                Material newMaterial = new Material { Name = material.Name, Category = material.Category, Extension = file.FileName.Split(".").Last() };
                var result = _materialService.AddMaterial(newMaterial, file);
                if (result != null)
                    return Ok();
            }
            return BadRequest("Материал с данным именем уже существует");
        }

        // POST: api/Material/add
        [HttpPost]
        [Route("add")]
        public IActionResult AddVersion([FromForm] UpdateMaterialDto material, [FromForm] IFormFile file)
        {
            if (material.Name != null && file != null)
            {
                var result = _materialService.AddVersion(material.Name, file);
                if (result != null)
                    return Ok();
            }
            return BadRequest("Не удалось найти материал с данным именем");
        }

        [HttpDelete]
        [Route("delete")]

        public IActionResult DeleteMaterial(string name)
        {
            if (name != null && _materialService.DeleteMaterial(name) != null)
            {
                return Ok();
            }
            return BadRequest("Не удалось найти материал с данным именем");
        }
    }
}
