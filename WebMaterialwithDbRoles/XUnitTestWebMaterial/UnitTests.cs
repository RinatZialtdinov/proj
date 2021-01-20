using System;
using Xunit;
using WebMaterial.DAL;
using WebMaterial.BLL;
using WebMaterial.DAL.Data;
using WebMaterial.DAL.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;

namespace WebMaterialTest
{
    public class UnitTests
    {
        //private MaterialService _materialService = new MaterialService(
        //    new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
        //        .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NewMaterialDb;Trusted_Connection=True;")
        //        .Options), new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json")
        //    .Build());

        [Fact]
        public void GetMaterialById_GetMaterialWithId1_NotNull()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.FindById(1)).Returns(new Material { Id = 1, Name = "Test", Category = (Category)1 });
            MaterialService materialService = new MaterialService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build(), mock.Object);

            var result = materialService.GetMaterialById(1);
            
            Assert.NotNull(result);
        }

        [Fact]
        public void GetMaterialByName_GetMaterialWithNameRINAT_NotNull()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.FindByName("RINAT")).Returns(new Material { Id = 1, Name = "RINAT", Category = (Category)1 });
            MaterialService materialService = new MaterialService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build(), mock.Object);

            var result = materialService.GetMaterialByName("RINAT");
            
            Assert.NotNull(result);
        }

        [Fact]

        public void ChangeMaterialCategory_GetMaterialWithEditCategory_Other()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.FindByName("Test")).Returns(new Material { Id = 1, Name = "Test", Category = (Category)2 });
            MaterialService materialService = new MaterialService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build(), mock.Object);

            var result = materialService.ChangeMaterialCategory("Test", (Category)1);

            Assert.Equal((Category)1, result.Category);
        }

        [Fact]

        public void AddMaterial_AddEmptyFile_NotNull()
        {
            var mock = new Mock<IRepository>();
            var material = new Material { Id = 1, Name = "RINAT", Category = (Category)1 };
            Material ret = null;
            mock.Setup(repo => repo.FindByName("RINAT")).Returns(ret);
            MaterialService materialService = new MaterialService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build(), mock.Object);
            IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("new")), 0, 0, "Data", "text1.txt");

            var result = materialService.AddMaterial(material, file);
            
            Assert.NotNull(result);
        }
        private List<Material> GetTestMaterials()
        {
            var materials = new List<Material>
            {
                new Material { Id=1, Name="Tom", Category=(Category)1},
                new Material { Id=2, Name="Alice", Category=(Category)1},
                new Material { Id=3, Name="Sam", Category=(Category)2},
                new Material { Id=4, Name="Kate", Category=(Category)1}
            };
            return materials;
        }
    }
}
