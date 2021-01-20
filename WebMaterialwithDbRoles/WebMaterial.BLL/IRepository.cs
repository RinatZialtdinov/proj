using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebMaterial.DAL.Models;
using Version = WebMaterial.DAL.Models.Version;

namespace WebMaterial.BLL
{
    public interface IRepository
    {
        public Material FindByName(string name);
        public Material FindById(int id);
        public List<Material> GetAllMaterials();
        public IQueryable<Material> SelectAllMaterials();
        public void AllSave();
        public void AddVersion(Version newVersion);
        public void AddMaterial(Material material);

    }
}
