using _1101113.BusinessManagers.Interfaces;
using _1101113.Models.AdminViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _1101113.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardBusinessManager _dashboardBusinessManager;

        // Constructor: Dependency Injection ของ DashboardBusinessManager
        public DashboardController(IDashboardBusinessManager dashboardBusinessManager)
        {
            _dashboardBusinessManager = dashboardBusinessManager;
        }

        // Action: ดึงข้อมูลจาก DashboardBusinessManager และส่งไปยัง View
        public async Task<IActionResult> Index()  // เปลี่ยนชื่อจาก Dashboard เป็น Index
        {
            // ส่ง ClaimsPrincipal ไปยัง DashboardBusinessManager
            var dashboardData = await _dashboardBusinessManager.GetDashboardData(User); // User คือ ClaimsPrincipal
            if (dashboardData == null)
            {
                return View("Error"); // หรือแสดงข้อความผิดพลาด
            }

            return View(dashboardData);  // ส่งข้อมูลไปยัง View
        }
    }
}
