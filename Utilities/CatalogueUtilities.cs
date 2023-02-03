using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// Danh mục enum hệ thống
    /// </summary>
    public class CatalogueEnums
    {
        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        public enum userStatus
        {
            notActivated,
            active,
            locked,
        }
        /// <summary>
        /// Giới tính
        /// </summary>
        public enum userGender
        {
            Unknown,
            Male,
            Female,
        }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public enum UnitType
        {
            /// <summary>
            /// Đơn vị chỉ
            /// </summary>
            chi,
            /// <summary>
            /// Đơn vị Gram
            /// </summary>
            gram
        }
        /// <summary>
        /// Trạng thái nữ trang, hột xoàn
        /// </summary>
        public enum StatusProduct
        {
            /// <summary>
            /// Chờ phê duyệt
            /// </summary>
            WaitingForApproval,
            /// <summary>
            /// Đã phê duyệt
            /// </summary>
            Approved,
            /// <summary>
            /// Đã bán
            /// </summary>
            Sold
        }

        /// <summary>
        /// Trạng thái phiếu thu chi
        /// </summary>
        public enum StatusRevenueAndExpenditure
        {
            /// <summary>
            /// Chờ phê duyệt
            /// </summary>
            WaitingForApproval,
            /// <summary>
            /// Đã phê duyệt
            /// </summary>
            Approved,
            /// <summary>
            /// Không được phê duyệt
            /// </summary>
            NotApproved
        }
        /// <summary>
        /// Trạng thái của khuyến mãi
        /// </summary>
        public enum DiscountStatus
        {
            /// <summary>
            /// Chưa kích hoạt
            /// </summary>
            notActivated,
            /// <summary>
            /// Đang diễn ra
            /// </summary>
            inProgress,
            /// <summary>
            /// Đã kết thúc
            /// </summary>
            end
        }
        /// <summary>
        /// Điều kiện áp dụng khuyễn mãi
        /// </summary>
        public enum DiscountConditionsApply
        {
            khongdieukien,
            nutrang,
            hotxoan
        }
        /// <summary>
        /// Loại khuyến mãi
        /// </summary>
        public enum DiscountType
        {
            giamtheosotien,
            giamtheophantram
        }
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        public enum StatusOrder
        {
            /// <summary>
            /// Chờ xác nhận
            /// </summary>
            ChoXacNhan,
            /// <summary>
            /// Đã xác nhận
            /// </summary>
            DaXacNhan,
            /// <summary>
            /// Đang xử lý
            /// </summary>
            DangXuLy,
            /// <summary>
            /// Đang giao hàng
            /// </summary>
            DangGiaoHang,
            /// <summary>
            /// Hoàn thành
            /// </summary>
            HoanThanh,
            /// <summary>
            /// Hoàn tiền cọc
            /// </summary>
            HoanTienCoc
        }
        public enum StatusTransaction
        {
            /// <summary>
            /// Chờ phê duyệt
            /// </summary>
            ChoPheDuyet,
            /// <summary>
            /// Đã phê duyệt
            /// </summary>
            DaPheDuyet,
            /// <summary>
            /// Hoàn thành
            /// </summary>
            HoanThanh
        }
        /// <summary>
        /// Loại đơn hàng
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// Mua hàng
            /// </summary>
            MuaHang,
            /// <summary>
            /// Bán hàng
            /// </summary>
            BanHang,
            /// <summary>
            /// Thu đổi hàng
            /// </summary>
            ThuDoiHang
        }

        /// <summary>
        /// Chế độ mặc định
        /// </summary>
        public enum OptionMode
        {
            Thu,
            Doi,
            HoTro
        }
        public class CatalogueCore
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<ObjectCatalogueCustom> Catalogue { get; set; }
        }
        public class ObjectJsonCustom
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
        }
        public class ObjectJsonRole
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int RoleNumberLevel { get; set; }
        }


        public class ObjectJsonUrl
        {

            public string UrlReferrer { get; set; }
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class ObjectTechnicalProduct
        {
            //public Guid TechnicalOptionId { get; set; }
            //public string TechnicalValue { get; set; }
            //public bool isFile { get; set; }
            public string FileName { get; set; }
            public string Link { get; set; }
        }


        public class ObjectFile
        {
            //public Guid TechnicalOptionId { get; set; }
            //public string TechnicalValue { get; set; }
            //public bool isFile { get; set; }
            public string FileNameSaveInSystem { get; set; }
            public string FileName { get; set; }
            public string Link { get; set; }
        }

        public class ObjectJobId
        {
            public string JobId1 { get; set; }
            public string JobId2 { get; set; }
        }


        public class ObjectCatalogueCustom
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }


        public class BidTicketStatusModel
        {
            public int? Status { get; set; }
        }

        public class BidTicketStatusListModel
        {
            public List<Guid> ListId { get; set; }
            public int? Status { get; set; }
        }

        public class NotificationUserModel
        {
            public Guid id { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
        }



        public static List<CatalogueCore> GetCatalogues()
        {
            List<CatalogueCore> catalogueCores = new List<CatalogueCore>();
            catalogueCores.Add(new CatalogueCore() { Id = 1, Name = "Trạng thái tài khoản", Catalogue = GetUserStatus() });
            catalogueCores.Add(new CatalogueCore() { Id = 2, Name = "Giới tính", Catalogue = GetUserGender() });
            catalogueCores.Add(new CatalogueCore() { Id = 3, Name = "Đơn vị tính", Catalogue = GetUnitType() });
            catalogueCores.Add(new CatalogueCore() { Id = 4, Name = "Trạng thái khuyến mãi", Catalogue = GetDiscountStatus() });
            catalogueCores.Add(new CatalogueCore() { Id = 5, Name = "Điều kiện áp dụng khuyến mãi", Catalogue = GetDiscountConditionsApply() });
            catalogueCores.Add(new CatalogueCore() { Id = 6, Name = "Loại khuyến mãi", Catalogue = GetDiscountType() });
            catalogueCores.Add(new CatalogueCore() { Id = 7, Name = "Trạng thái nữ trang, hột xoàn", Catalogue = GetStatusProduct() });
            catalogueCores.Add(new CatalogueCore() { Id = 8, Name = "Trạng thái phiếu thu chi", Catalogue = GetStatusRevenueAndExpenditure() });
            //catalogueCores.Add(new CatalogueCore() { Id = 9, Name = "Trạng thái đơn hàng", Catalogue = GetStatusOrder() });
            catalogueCores.Add(new CatalogueCore() { Id = 10, Name = "Trạng thái đơn hàng", Catalogue = GetStatusTransaction() });
            catalogueCores.Add(new CatalogueCore() { Id = 11, Name = "Loại đơn", Catalogue = GetOrderType() });
            return catalogueCores;
        }
        /// <summary>
        /// Danh sách trạng thái của tài khoản
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetUserStatus()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)userStatus.notActivated,
                    Name = "Chưa kích hoạt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)userStatus.active,
                    Name = "Đang kích hoạt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)userStatus.locked,
                    Name = "Đã khóa"
                }
            };
        }
        /// <summary>
        /// Danh sách giới tính
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetUserGender()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)userGender.Unknown,
                    Name = "Không xác định"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)userGender.Male,
                    Name = "Nam"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)userGender.Female,
                    Name = "Nữ"
                }
            };
        }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetUnitType()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)UnitType.chi,
                    Name = "Chỉ"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)UnitType.gram,
                    Name = "Gram"
                }
            };
        }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetDiscountStatus()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountStatus.notActivated,
                    Name = "Chưa kích hoạt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountStatus.inProgress,
                    Name = "Đang diễn ra"
                },
                 new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountStatus.end,
                    Name = "Đã kết thúc"
                }
            };
        }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetDiscountConditionsApply()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountConditionsApply.khongdieukien,
                    Name = "Không điều kiện"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountConditionsApply.nutrang,
                    Name = "Nữ trang"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountConditionsApply.hotxoan,
                    Name = "Hột xoàn"
                }
            };
        }

        /// <summary>
        /// Loại khuyến mãi
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetDiscountType()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountType.giamtheosotien,
                    Name = "Giảm theo số tiền"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)DiscountType.giamtheophantram,
                    Name = "Giảm theo phần trăm"
                }
            };
        }
        /// <summary>
        /// Trạng thái nữ trang, hột xoàn
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetStatusProduct()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusProduct.WaitingForApproval,
                    Name = "Chờ phê duyệt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusProduct.Sold,
                    Name = "Đã bán"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusProduct.Approved,
                    Name = "Đã phê duyệt"
                }
            };
        }

        /// <summary>
        /// Trạng thái phiếu thu chi
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetStatusRevenueAndExpenditure()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusRevenueAndExpenditure.WaitingForApproval,
                    Name = "Chờ phê duyệt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusRevenueAndExpenditure.Approved,
                    Name = "Đã phê duyệt"
                },
                 new ObjectCatalogueCustom()
                {
                    Id = (int)StatusRevenueAndExpenditure.NotApproved,
                    Name = "Không phê duyệt"
                }
            };
        }
        /// <summary>
        /// Trạng thái đơn hàng
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetStatusOrder()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.ChoXacNhan,
                    Name = "Chờ xác nhận"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.DaXacNhan,
                    Name = "Đã xác nhận"
                },
                 new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.DangXuLy,
                    Name = "Đang xử lý"
                },
                 new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.DangGiaoHang,
                    Name = "Đang giao hàng"
                },
                  new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.HoanThanh,
                    Name = "Hoàn thành"
                },
                    new ObjectCatalogueCustom()
                {
                    Id = (int)StatusOrder.HoanTienCoc,
                    Name = "Hoàn tiền cọc"
                }
            };
        }

        /// <summary>
        /// Trạng thái giao dịch
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetStatusTransaction()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusTransaction.ChoPheDuyet,
                    Name = "Chờ phê duyệt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusTransaction.DaPheDuyet,
                    Name = "Đã phê duyệt"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)StatusTransaction.HoanThanh,
                    Name = "Hoàn thành"
                }
            };
        }

        /// <summary>
        /// Loại đơn
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetOrderType()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)OrderType.MuaHang,
                    Name = "Mua hàng"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)OrderType.BanHang,
                    Name = "Bán hàng"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)OrderType.ThuDoiHang,
                    Name = "Thu đổi hàng"
                }
            };
        }

        /// <summary>
        /// Chế độ thu đổi hỗ trợ
        /// </summary>
        /// <returns></returns>
        public static List<ObjectCatalogueCustom> GetOptionMode()
        {
            return new List<ObjectCatalogueCustom>()
            {
                new ObjectCatalogueCustom()
                {
                    Id = (int)OptionMode.Thu,
                    Name = "Thu"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)OptionMode.Doi,
                    Name = "Đổi"
                },
                new ObjectCatalogueCustom()
                {
                    Id = (int)OptionMode.HoTro,
                    Name = "Hỗ trợ"
                }
            };
        }
    }
}
