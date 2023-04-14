using System.Web.Mvc;

namespace BanDienThoaiDiDong.Areas.QLDH
{
    public class QLDHAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get 
            {
                return "QLDH";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QLDH_default",
                "QLDH/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}