using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task5;

namespace task5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        IWebHostEnvironment _appEnvironmnet;
        public List<string> Categories = new List<string> { "Presentation", "Application", "Other" };

        public MaterialsController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironmnet = appEnvironment;
        }

        // GET: api/Materials/file
        [HttpGet("{Name}")]
        public async Task<ActionResult<Material>> GetMaterials(string Name)
        {
            Material material;

            if ((material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == Name)) != null)
            {
                return material;
                //return _context.Materials.Include(p => p.Versions).ToList();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("download")]
        public IActionResult GetFile(string Name, int? Version)
        {
            Material material;
            byte[] mas;

            if ((material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == Name)) != null)
            {
                string Path;

                if (Version != null)
                {
                    Path = _appEnvironmnet.ContentRootPath + $"/Files/{material.Name}_" + Version;
                    mas = System.IO.File.ReadAllBytes(Path);
                    return File(mas, "application/octet-stream", material.Name);
                }
                else
                {
                    Path = _appEnvironmnet.ContentRootPath + $"/Files/{material.Name}_" + material.Versions.Count();
                    mas = System.IO.File.ReadAllBytes(Path);
                    return File(mas, "application/octet-stream", material.Name);
                }
            }
            return NoContent();
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Material>>> GetFilteredMaterials(string category)
        {
            var materials = from m in _context.Materials
                            select m;
            materials = _context.Materials.Include(p => p.Versions);
            if (Categories.Contains(category))
            {
                materials = materials.Where(s => s.Category == category);
                return materials.ToList();
            }    
            return BadRequest();
        }
        // GET: api/Materials/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Material>> GetMaterial(int id)
        //{
            //var material = await _context.Materials.FindAsync(id);

            //if (material == null)
            //{
                //return NotFound();
            //}

            //return material;
        //}

        // PUT: api/Materials/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPatch]
        public async Task<ActionResult<Material>> PatchMaterial(string Name, string Category)
        {
            Material material;

            if ((material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == Name)) != null
                && Categories.Contains(Category))
            {
                material.Category = Category;
                _context.SaveChanges();
                return material;
            }
            return BadRequest();
        }

        // POST: api/Materials
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult> PostMaterial(NewMaterialDto material)
        {
            Material item;
            Version newVersion;
            if (Categories.Contains(material.Category) && material.Name != null && material.File != null 
                && _context.Materials.FirstOrDefault(p => p.Name == material.Name) == null)
            {
                item = new Material { Name = material.Name, Category = material.Category };
                newVersion = new Version
                {
                    Material = item,
                    Path = _appEnvironmnet.ContentRootPath + $"/Files/{item.Name}_1", //_appEnvironmnet.WebRootPath "/Files/" + item.Name + "1",
                    Release = 1,
                    Size = material.File.Length,
                    UploadDateTime = DateTime.Now
                };
                //item.Versions.Add(newVersion);
                System.IO.File.WriteAllBytes(newVersion.Path, material.File);
                //using (var fileStream = new FileStream(_appEnvironmnet.WebRootPath + newVersion.Path, FileMode.Create))
                //{
                //await material.File.CopyToAsync(fileStream);
                //}
                _context.Materials.Add(item);
                _context.Versions.Add(newVersion);
                _context.SaveChanges();
                return CreatedAtAction("GetMaterials", new { id = item.Id }, item);
            }
            //Material item = new Material { Name = material.Name, Category = material.Category };

            return BadRequest();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> UpdateMaterial(UpdateMaterialDto material)
        {
            Material item;
            Version newVersion;
            if (material.Name != null && material.File != null && 
                _context.Materials.FirstOrDefault(p => p.Name == material.Name) != null)
            {
                item = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == material.Name);
                newVersion = new Version
                {
                    Material = item,
                    Path = _appEnvironmnet.ContentRootPath + $"/Files/{item.Name}_" + (item.Versions.Count() + 1), //"/Files/" + item.Name + (char)item.Versions.Count + 1,
                    Release = item.Versions.Count + 1,
                    Size = material.File.Length,
                    UploadDateTime = DateTime.Now
                };
               // item.Versions.Add(newVersion);
                System.IO.File.WriteAllBytes(newVersion.Path, material.File);
                //using (var fileStream = new FileStream(_appEnvironmnet.WebRootPath + newVersion.Path, FileMode.Create))
                //{
                    //await material.File.CopyToAsync(fileStream);
                //}
                //_context.Materials.Add(item);
                _context.Versions.Add(newVersion);
                _context.SaveChanges();
                return CreatedAtAction("GetMaterials", new {Id = newVersion.Id }, newVersion);
            }
            return BadRequest();
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Material>> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return material;
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
