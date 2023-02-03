using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public class ValidateUserName
    {
        public static bool IsValidUserName(string userName)
        {
            bool result = false;
            bool isPhoneNumber = IsPhoneNumber(userName);
            bool isEmail = IsEmail(userName);
            result = isPhoneNumber || isEmail;
            return result;
        }


        public static bool IsEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Kiểm tra số điện thoại
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[0-9]+${9,11}").Success;
        }
    }
}
