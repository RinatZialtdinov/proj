using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private List<string> Categories = new List<string> { "Presentation", "Application", "Other" };

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
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
            return BadRequest();
        }

        // GET: api/Material/{name}
        [HttpGet]
        [Route("name")]
        public IActionResult GetMaterialByName(string name)
        {
            var material = _materialService.GetMaterialByName(name);
            if (material != null)
                return Ok(material);
            return BadRequest();
        }

        [HttpGet]
        [Route("filter")]
        public ActionResult<List<Material>> GetFilteredMaterials(string category)
        {
            if (Categories.Contains(category))
            {
                var materials = _materialService.GetFilteredMaterials(category);
                if (materials != null)
                    return materials.ToList();
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("download")]
        public IActionResult DownloadFile(string name, int? version)
        {
            var result = _materialService.DownloadFile(name, version);
            if (result != null)
            {
                return File(result, "application/octet-stream", name);
            }
            return BadRequest();
        }

        [HttpPatch]
        public ActionResult<Material> ChangeMaterialCategory(string name, string category)
        {
            if (Categories.Contains(category))
            {
                var material = _materialService.ChangeMaterialCategory(name, category);
                if (material != null)
                {
                    return Ok(material);
                }
            }
            return BadRequest();
        }

        // POST: api/Material
        [HttpPost]
        public IActionResult AddMaterial([FromForm] NewMaterialDto material, [FromForm] IFormFile file)
        {
            if (material.Name != null && material.Category != null && file != null
                && file.Length < 2147483648 && Categories.Contains(material.Category))
            {
                Material newMaterial = new Material { Name = material.Name, Category = material.Category };
                var result = _materialService.AddMaterial(newMaterial, file);
                if (result != null)
                    return Ok();
            }
            return BadRequest();
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
            return BadRequest();
        }


        // PUT: api/Material
        //[HttpPut]
        //public IActionResult UpdateMaterial(/*NewVersionMaterialDTO model*/)
        //{
        //    return Ok();
        //}
        //// PUT: api/Material/Base64
        //[Route("Base64")]
        //[HttpPut]
        //public IActionResult UpdateBase64Material(/*NewVersionMaterialDTO model*/)
        //{
        //    return Ok();
        //}

        // DELETE: api/Material/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMaterial(int id)
        {
            return Ok();
        }


        // GET: api/Material/Download/{name}
        //[HttpGet("download/{name}")]
        //public IActionResult DownloadMaterial(string name, int? version)
        //{
        //    return Ok();
        //}
    }
}
