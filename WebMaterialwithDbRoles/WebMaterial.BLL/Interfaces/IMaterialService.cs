﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WebMaterial.DAL.Models;
using Version = WebMaterial.DAL.Models.Version;

namespace WebMaterial.BLL
{
    public interface IMaterialService
    {
        //public IEnumerable<Material> GetMaterials();
        //public IEnumerable<Material> GetMaterialsByCategory(int category);
        public Material GetMaterialByName(string name);
        public Material GetMaterialById(int id);
        public IList<Material> GetAllMaterials();
        public IList<Material> GetFilteredMaterials(Category category);
        public Material ChangeMaterialCategory(string name, Category category);

        public Version AddMaterial(Material material, IFormFile file);
        public Version AddVersion(string name, IFormFile file);
        public string DeleteMaterial(string name);
    }
}
