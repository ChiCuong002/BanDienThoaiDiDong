using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.NVQLDonHang
{
    public class NVQLDonHangAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NVQLDonHang";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NVQLDonHang_default",
                "NVQLDonHang/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}