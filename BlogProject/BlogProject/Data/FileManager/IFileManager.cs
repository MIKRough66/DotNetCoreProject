﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using System.IO;

namespace BlogProject.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);

        Task<string> SaveImage(IFormFile image);
        bool RemoveImage(string image);
    }
}
