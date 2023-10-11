using Microsoft.AspNetCore.Mvc;
using WebAppl.Data;
using WebAppl.Models;
namespace WebAppl.ViewComponents
{
    public class MajorViewComponent:ViewComponent
    {
        SchoolContext db;
        List<Major> majors;
        public MajorViewComponent(SchoolContext _context)
        {
            db=_context;
            majors=db.Majors.ToList();
        }

        public  async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
