using Microsoft.AspNetCore.Hosting;
using System;
using task5;
using task5.Controllers;
using Xunit;

namespace XUnitTestApp
{
    public class UnitTest1
    {
        private readonly ApplicationContext _context;
        private IWebHostEnvironment _appEnvironment;

        [Fact]
        public void Test1()
        {
            MaterialsController controller = new MaterialsController(_context, _appEnvironment);

            var result = controller.GetMaterials("iswork");


        }
    }
}
