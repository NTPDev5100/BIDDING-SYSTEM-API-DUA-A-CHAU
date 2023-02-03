using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class AppDomainImportResult : AppDomainResult
    {
        /// <summary>
        /// File kết quả trả về
        /// </summary>
        public byte[] resultFile { get; set; }

        /// <summary>
        /// Tên file kết quả
        /// </summary>
        public string downloadFileName { get; set; }
    }
}
