using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Contants
    {
        #region ControllerName

        /// <summary>
        /// CONTROLLER QUẢN LÝ THÔNG TIN LIÊN HỆ
        /// </summary>
        public const string ContactCustomerController = "ContactCustomer";

        /// <summary>
        /// CONTROLLER QUẢN LÝ DỰ ÁN
        /// </summary>
        public const string ProjectController = "Project";

        /// <summary>
        /// CONTROLLER QUẢN LÝ THÔNG TIN CÔNG TY
        /// </summary>
        public const string CatalogueCompanyController = "CatalogueCompany";

        #endregion


        /// <summary>
        /// METHOD NOTIFY CHO TRANG LIÊN HỆ KHÁCH HÀNG
        /// </summary>
        public const string NOTIFY_SEND_CONTACT_CUSTOMER = "send-contact-customer";

        #region FOLDER UPLOAD NAME

        /// <summary>
        /// Tên thư mục upload của thông tin liên hệ
        /// </summary>
        public const string COLOR_TASK_FOLDER = "ColorTask";

        /// <summary>
        /// Tên thư mục upload của thông tin liên hệ
        /// </summary>
        public const string CONTACT_CUSTOMER_FOLDER = "ContactCustomer";

        /// <summary>
        /// Tên thư mục upload của project
        /// </summary>
        public const string PROJECT_FOLDER = "DocumentProject";

        public const string NEWFEED_FOLDER = "DocumentNewFeed";

        #endregion

        #region ROLE CODE

        /// <summary>
        /// Leader
        /// </summary>
        public const string USER_GROUP_LEADER = "LEADER";

        /// <summary>
        /// Dev
        /// </summary>
        public const string USER_GROUP_DEVELOPER = "DEVELOPER";

        /// <summary>
        /// Nhóm khách hàng
        /// </summary>
        public const string USER_GROUP_CUSTOMER = "CUSTOMER";


        #endregion

        #region SMS TEMPLATE

        /// <summary>
        /// SMS template gửi mật khẩu cho user
        /// </summary>
        public const string SMS_PASSWORD_TEMPLATE = "SMS_PASSWORD_TEMPLATE";

        #endregion

        #region SignalR
        /// <summary>
        /// Signalr có thông báo mới
        /// </summary>
        public const string SR_NOTIFICATION = "SR_NOTIFICATION";


        /// <summary>
        /// signalr khi comment vào bài viết
        /// </summary>
        public const string SR_POST_COMMENT = "SR_POST_COMMENT";

        /// <summary>
        /// signalr khi comment xóa
        /// </summary>
        public const string SR_POST_COMMENT_DELETE = "SR_POST_COMMENT_DELETE";

        public const string UPDATE_BY_SEVER = "server";

        #endregion
    }
}
