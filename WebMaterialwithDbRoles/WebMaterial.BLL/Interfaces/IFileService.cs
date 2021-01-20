using System;
using System.Collections.Generic;
using System.Text;

namespace WebMaterial.BLL
{
    public interface IFileService
    {
        public byte[] DownloadFileFromSystem(string name, int? version);
    }
}
