using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebMaterial.DAL.Data;
using WebMaterial.DAL.Models;
using Version = WebMaterial.DAL.Models.Version;

namespace WebMaterial.BLL
{
    public class Repository : IRepository
    {
        protected readonly ApplicationContext _context;
        public Repository(ApplicationContext context)
        {
            _context = context;
        }

        public Material FindByName(string name)
        {
            var material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == name);
            if (material != null)
                return material;
            return null;
        }

        public Material FindById(int id)
        {
            var material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Id == id);
            if (material != null)
                return material;
            return null;
        }

        public List<Material> GetAllMaterials()
        {
            var materials =  _context.Materials.Include(p => p.Versions).ToList<Material>();
            if (materials != null)
                return materials;
            return null;
        }

        public IQueryable<Material> SelectAllMaterials()
        {
            var materials = from m in _context.Materials.Include(p => p.Versions)
                            select m;
            if (materials != null)
                return materials;
            return null;
        }

        public void AllSave()
        {
            _context.SaveChanges();
        }
        public void AddVersion(Version newVersion)
        {
            _context.Versions.Add(newVersion);
        }
        public void AddMaterial(Material material)
        {
            _context.Materials.Add(material);
        }
    }
}
