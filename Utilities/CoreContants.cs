using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class CoreContants
    {
        //public const string Get = "Get";
        //public const string Post = "Post";
        //public const string Put = "Put";
        //public const string Delete = "Delete";

        public const string ViewAll = "ViewAll";
        public const string Delete = "Delete";
        public const string AddNew = "AddNew";
        public const string View = "View";
        public const string FullControl = "FullControl";
        public const string Update = "Update";
        public const string Approve = "Approve";
        public const string Import = "Import";
        public const string Upload = "Upload";
        public const string Download = "Download";
        public const string DeleteFile = "DeleteFile";
        public const string Export = "Export";

        //public const string FullControl = "FullControl";
        //public const string Approve = "Approve";
        //public const string DeleteFile = "DeleteFile";



        //Folder name
        public const string UPLOAD_FOLDER_NAME = "Upload";
        public const string TEMP_FOLDER_NAME = "Temp";
        public const string TEMPLATE_FOLDER_NAME = "Template";
        public const string FILE_LOG_FOLDER_NAME = "FileLog";
        public const string QR_CODE_FOLDER_NAME = "QRCode";


        //File name
        public const string CATALOGUE_TEMPLATE_NAME = "CatalogueTemplate.xlsx";
        public const string CREATE_NEW_PROVIDER_EMAIL_TEMPLATE_NAME = "CreateNewProviderEmailTemplate.html";
        public const string ACTIVE_PROVIDER_EMAIL_TEMPLATE_NAME = "ActiveProviderEmailTemplate.html";
        public const string BIDDING_TICKET_RESULT_EMAIL_TEMPLATE_NAME = "BiddingTicketResultEmailTemplate.html";
        public const string LOG_NAME = "log.txt";


        public const string GET_TOTAL_NOTIFICATION = "get-total-notification";

        /// <summary>
        /// Danh mục quyền
        /// </summary>
        //public enum PermissionContants
        //{
        //    Get = 1,
        //    Post = 2,
        //    Put = 3,
        //    Delete = 4
        //}
        //public enum userStatus
        //{
        //    notActivated,
        //    active,
        //    locked,

        //}

        public enum StatucBiddingTicket
        {
            ChoDuyet = 0,
            ChoKetQua = 1,
            TrungThau = 2,
            RotThau = 3,
            Huy
        }

        public enum StatusBiddingSession
        {
            ChuaDienRa = 0,
            DangDienRa = 1,
            DaKetThuc = 2,
            DaDong = 3
        }


        public enum RoleNumberLevel
        {
            KhachHang = 1,
            NhanVien = 2,
            QuanLy = 3
        }


        public enum TypeFileExcelBidTicket
        {
            DanhSachLichSuTheoPhien = 1,
            DanhSachTong = 2
        }

        public enum TypeNotification
        {

            PhienThauBatDau = 1,
            PhienThauKetThuc = 2,
            TrungThau = 3,
            RotThau = 4
        }

        #region Catalogue Name
        /// <summary>
        /// Phường
        /// </summary>
        public const string WARD_CATALOGUE_NAME = "Ward";

        /// <summary>
        /// Quốc gia
        /// </summary>
        public const string COUNTRY_CATALOGUE_NAME = "Country";

        /// <summary>
        /// Quận
        /// </summary>
        public const string DISTRICT_CATALOGUE_NAME = "District";

        /// <summary>
        /// Thành phố
        /// </summary>
        public const string CITY_CATALOGUE_NAME = "City";

        /// <summary>
        /// Dân tộc
        /// </summary>
        public const string NATION_CATALOGUE_NAME = "Nation";

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public const string NOTIFICATION_TYPE_CATALOGUE_NAME = "NotificationType";
        #endregion

        #region SMS Template
        /// <summary>
        /// Xác nhận OTP SMS
        /// </summary>
        public const string SMS_XNOTP = "XNOTP";
        #endregion

        #region Email Template
        #endregion
    }
}
