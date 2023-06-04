using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.QLDonHang
{
    public class QLDonHangAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QLDonHang";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QLDonHang_default",
                "QLDonHang/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}