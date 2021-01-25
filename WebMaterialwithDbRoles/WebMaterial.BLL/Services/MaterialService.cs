using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using WebMaterial.DAL.Data;
using WebMaterial.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Version = WebMaterial.DAL.Models.Version;
using System.Threading.Tasks;

namespace WebMaterial.BLL
{
    public class MaterialService : IMaterialService
    {
        //private readonly ApplicationContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _config;

        public MaterialService(IConfiguration config, IRepository repository)
        {
            //_context = context;
            _config = config;
            _repository = repository;
        }

        public IList<Material> GetAllMaterials()
        {
            return _repository.GetAllMaterials();
        }
        public Material GetMaterialByName(string name)
        {
            var material = _repository.FindByName(name);
            if (material == null)
                return null;
            return material;
        }

        public Material GetMaterialById(int id)
        {
            var material = _repository.FindById(id);
            if (material == null)
                return null;
            return material;
        }

        public Version AddMaterial(Material material, IFormFile file)
        {
            Version newVersion;
            if (_repository.FindByName(material.Name) == null)
            {
                newVersion = new Version
                {
                    Material = material,
                    Path = _config.GetValue<string>("PathFiles") + material.Name + "_1" + "." + material.Extension,
                    Release = 1,
                    Size = file.Length,
                    UploadDateTime = DateTime.Now
                };
                var filestream = new FileStream(newVersion.Path, FileMode.Create);
                
                    file.CopyTo(filestream);
                _repository.AddMaterial(material);
                _repository.AddVersion(newVersion);
                _repository.AllSave();
                return newVersion;
            }
            return null;
        }

        public Version AddVersion(string name, IFormFile file)
        {
            Material material = _repository.FindByName(name);
            Version newVersion;
            if (material != null)
            {
                newVersion = new Version
                {
                    Material = material,
                    Path = _config.GetValue<string>("PathFiles") + material.Name + "_" + (material.Versions.Count() + 1) + '.' + file.FileName.Split('.')[1],
                    Release = material.Versions.Count() + 1,
                    Size = file.Length,
                    UploadDateTime = DateTime.Now
                };
                using (var filestream = new FileStream(newVersion.Path, FileMode.Create))
                {
                    file.CopyToAsync(filestream);
                }
                _repository.AddVersion(newVersion);
                _repository.AllSave();
                return newVersion;
            }
            return null;
        }
        public IList<Material> GetFilteredMaterials(Category category)
        {
            var materials = _repository.SelectAllMaterials();
            var filteredMaterials = materials.Where(s => s.Category == category);
            if (filteredMaterials != null)
                return filteredMaterials.ToList();
            return null;
        }

        public Material ChangeMaterialCategory(string name, Category category)
        {
            var material = _repository.FindByName(name);
            if (material != null)
            {
                material.Category = category;
                _repository.AllSave();
                return material;
            }
            return null;
        }

        public string DeleteMaterial(string name)
        {
            var material = _repository.FindByName(name);
            if (material != null)
            {
                _repository.DeleteMaterial(material);
                return "ОК";
            }
            return null;
        }
    }
}
