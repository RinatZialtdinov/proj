﻿using Microsoft.EntityFrameworkCore;
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
            //return _context.Materials.Include(p => p.Versions).ToList<Material>();
            return _repository.GetAllMaterials();
        }
        public Material GetMaterialByName(string name)
        {
            var material = _repository.FindByName(name);
            //var material = _context.Materials.Include(p => p.Versions).Where(p => p.Name == name).FirstOrDefault();
            if (material == null)
                return null;
            return material;
        }

        public Material GetMaterialById(int id)
        {
            var material = _repository.FindById(id);
            //var material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Id == id);
            if (material == null)
                return null;
            return material;
        }

        public Version AddMaterial(Material material, IFormFile file)
        {
            Version newVersion;
            //if (_context.Materials.FirstOrDefault(p => p.Name == material.Name) == null)
            if (_repository.FindByName(material.Name) == null)
            {
                newVersion = new Version
                {
                    Material = material,
                    Path = _config.GetValue<string>("PathFiles") + material.Name + "_1" + "." + material.Extensio,
                    Release = 1,
                    Size = file.Length,
                    UploadDateTime = DateTime.Now
                };
                var filestream = new FileStream(newVersion.Path, FileMode.Create);
                
                    file.CopyTo(filestream);

                //_context.Materials.Add(material);
                //_context.Versions.Add(newVersion);
                //_context.SaveChanges();
                _repository.AddMaterial(material);
                _repository.AddVersion(newVersion);
                _repository.AllSave();
                return newVersion;
            }
            return null;
        }

        public Version AddVersion(string name, IFormFile file)
        {
            //Material material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == name);
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
                //_context.Versions.Add(newVersion);
                //_context.SaveChanges();
                _repository.AddVersion(newVersion);
                _repository.AllSave();
                return newVersion;
            }
            return null;
        }
        public IList<Material> GetFilteredMaterials(Category category)
        {
            //var materials = from m in _context.Materials.Include(p => p.Versions)
            //               select m;
            var materials = _repository.SelectAllMaterials();
            var filteredMaterials = materials.Where(s => s.Category == category);
            if (filteredMaterials != null)
                return filteredMaterials.ToList();
            return null;
        }

        public Material ChangeMaterialCategory(string name, Category category)
        {
            //var material = _context.Materials.Include(p => p.Versions).Where(p => p.Name == name).FirstOrDefault();
            var material = _repository.FindByName(name);
            if (material != null)
            {
                material.Category = category;
                _repository.AllSave();
                //_context.SaveChanges();
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

        //public byte[] DownloadFile(string name, int? version)
        //{
        //    string Path;
        //    byte[] mas;
        //    //var material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == name);
        //    var material = _repository.FindByName(name);
        //    if (material != null)
        //    {
        //        if (version != null)
        //        {
        //            Path = _config.GetValue<string>("PathFiles") + material.Name + "_" + version;
        //            mas = System.IO.File.ReadAllBytes(Path);
        //            return mas;
        //            //return material;
        //            //return File(mas, "application/octet-stream", material.Name);
        //        }
        //        else
        //        {
        //            Path = _config.GetValue<string>("PathFiles") + material.Name + "_" + material.Versions.Count();
        //            mas = System.IO.File.ReadAllBytes(Path);
        //            return mas;
        //            //return material;
        //            //return File(mas, "application/octet-stream", material.Name);
        //        }
        //    }
        //    return null;
        //}

        //public IEnumerable<Material> GetMaterials()
        //{
        //    var ret = _context.Materials.Include(p => p.Versions); 
        //    if (ret == null)
        //        throw new Exception("No materials");
        //    return ret;
        //}

        //public IEnumerable<Material> GetMaterialsByCategory(int category)
        //{
        //    var ret = _context.Materials.Include(p => p.Versions).Where(p => (int)p.Category == category);
        //    if (ret == null)
        //        throw new Exception("Wrong category");
        //    return ret;
        //}
    }
}