using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class ContentFileTypeUtilities
    {
        public static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
