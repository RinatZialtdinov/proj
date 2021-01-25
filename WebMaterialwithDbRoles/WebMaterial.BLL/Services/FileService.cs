using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebMaterial.BLL
{
    public class FileService : IFileService
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _config;
        public FileService(IConfiguration config, IRepository repository)
        {
            //_context = context;
            _config = config;
            _repository = repository;
        }
        public byte[] DownloadFileFromSystem(string name, int? version)
        {
            string Path;
            byte[] mas;
            //var material = _context.Materials.Include(p => p.Versions).FirstOrDefault(p => p.Name == name);
            var material = _repository.FindByName(name);
            if (material != null)
            {
                if (version != null)
                {
                    Path = _config.GetValue<string>("PathFiles") + material.Name + "_" + version + '.' + material.Extension;
                    mas = System.IO.File.ReadAllBytes(Path);
                    return mas;
                    //return material;
                    //return File(mas, "application/octet-stream", material.Name);
                }
                else
                {
                    Path = _config.GetValue<string>("PathFiles") + material.Name + "_" + material.Versions.Count() + '.' + material.Extension;
                    mas = System.IO.File.ReadAllBytes(Path);
                    return mas;
                    //return material;
                    //return File(mas, "application/octet-stream", material.Name);
                }
            }
            return null;
        }
    }
}
