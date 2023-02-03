using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogUtilities
    {
        /// <summary>
        /// Ghi file log lỗi
        /// </summary>
        /// <param name="page">Trang lỗi</param>
        /// <param name="function">Hàm lỗi</param>
        /// <param name="loginId">Id user đang đăng nhập</param>
        /// <param name="contentError">Nội dung lỗi</param>
        public static void WriteLog(string page, string function, Guid loginId, string contentError)
        {
            string name = DateTime.Now.ToString("dd-mm-yyyy");
            string content = "PageError:" + page + "\nFunctionError:" + function + "\nLoginID:" + loginId + "\nMessageError:" + contentError + "\n" + DateTime.Now.ToString("dd/mm/yyyy") + "\n" + Environment.NewLine;
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            var path = Path.Combine(appRoot, CoreContants.FILE_LOG_FOLDER_NAME, CoreContants.LOG_NAME);
            File.AppendAllText(path, content);
        }
    }
}
