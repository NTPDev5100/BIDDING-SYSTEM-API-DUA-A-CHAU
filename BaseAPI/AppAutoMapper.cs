using AutoMapper;
using Entities;
using Entities.Catalogue;
using Models.Catalogue;
using Request.Catalogue.CatalogueCreate;
using Request.Catalogue.CatalogueUpdate;
using Request.RequestCreate;
using Request.RequestUpdate;
using Utilities;

namespace Models.AutoMapper
{
    public class AppAutoMapper : Profile
    {
        public AppAutoMapper()
        {
            //người dùng
            CreateMap<UserModel, tbl_Users>().ReverseMap();
            CreateMap<UserCreate, tbl_Users>().ReverseMap();
            CreateMap<UserUpdate, tbl_Users>().ReverseMap();
            CreateMap<PagedList<UserModel>, PagedList<tbl_Users>>().ReverseMap();

            //quyền
            CreateMap<RoleModel, tbl_Role>().ReverseMap();
            CreateMap<RoleCreate, tbl_Role>().ReverseMap();
            CreateMap<RoleUpdate, tbl_Role>().ReverseMap();
            CreateMap<PagedList<RoleModel>, PagedList<tbl_Role>>().ReverseMap();

            //menu
            CreateMap<MenuModel, tbl_Menu>().ReverseMap();
            CreateMap<RequestMenuCreateModel, tbl_Menu>().ReverseMap();
            CreateMap<RequestMenuUpdateModel, tbl_Menu>().ReverseMap();
            CreateMap<PagedList<MenuModel>, PagedList<tbl_Menu>>().ReverseMap();

            //sản phẩm
            CreateMap<ProductModel, tbl_Products>().ReverseMap();
            CreateMap<ProductsCreate, tbl_Products>().ReverseMap();
            CreateMap<ProductsUpdate, tbl_Products>().ReverseMap();
            CreateMap<PagedList<ProductModel>, PagedList<tbl_Products>>().ReverseMap();

            //Nhà cung cấp
            CreateMap<ProviderModel, tbl_Providers>().ReverseMap();
            CreateMap<ProviderCreate, tbl_Providers>().ReverseMap();
            CreateMap<ProviderUpdate, tbl_Providers>().ReverseMap();
            CreateMap<PagedList<ProviderModel>, PagedList<tbl_Providers>>().ReverseMap();

            //Gói thầu
            CreateMap<BiddingModel, tbl_Biddings>().ReverseMap();
            CreateMap<BiddingsCreate, tbl_Biddings>().ReverseMap();
            CreateMap<BiddingsUpdate, tbl_Biddings>().ReverseMap();
            CreateMap<PagedList<BiddingModel>, PagedList<tbl_Biddings>>().ReverseMap();

            //Phiên đấu thầu
            CreateMap<BiddingSessionModel, tbl_BiddingSessions>().ReverseMap();
            CreateMap<BiddingSessionCreate, tbl_BiddingSessions>().ReverseMap();
            CreateMap<BiddingSessionUpdate, tbl_BiddingSessions>().ReverseMap();
            CreateMap<PagedList<BiddingSessionModel>, PagedList<tbl_BiddingSessions>>().ReverseMap();

            //Phiếu đấu thầu
            CreateMap<BiddingTicketModel, tbl_BiddingTickets>().ReverseMap();
            CreateMap<BiddingTicketCreate, tbl_BiddingTickets>().ReverseMap();
            CreateMap<BiddingTicketUpdate, tbl_BiddingTickets>().ReverseMap();
            CreateMap<PagedList<BiddingTicketModel>, PagedList<tbl_BiddingTickets>>().ReverseMap();

            //Thông báo
            CreateMap<NotificationModel, tbl_Notification>().ReverseMap();
            CreateMap<NotificationCreate, tbl_Notification>().ReverseMap();
            CreateMap<NotificationUpdate, tbl_Notification>().ReverseMap();
            CreateMap<PagedList<NotificationModel>, PagedList<tbl_Notification>>().ReverseMap();

            //Loại kỹ thuật
            CreateMap<TechnicalOptionModel, tbl_TechnicalOptions>().ReverseMap();
            CreateMap<TechnicalOptionCreate, tbl_TechnicalOptions>().ReverseMap();
            CreateMap<TechnicalOptionUpdate, tbl_TechnicalOptions>().ReverseMap();
            CreateMap<PagedList<TechnicalOptionModel>, PagedList<tbl_TechnicalOptions>>().ReverseMap();

            //Kỹ thuật sản phẩm
            CreateMap<TechnicalProductModel, tbl_TechnicalProduct>().ReverseMap();
            CreateMap<TechnicalProductCreate, tbl_TechnicalProduct>().ReverseMap();
            CreateMap<TechnicalProductUpdate, tbl_TechnicalProduct>().ReverseMap();
            CreateMap<PagedList<TechnicalProductModel>, PagedList<tbl_TechnicalProduct>>().ReverseMap();


        }
    }
}