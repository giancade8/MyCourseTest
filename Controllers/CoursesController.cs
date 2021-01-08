using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;

        }

        public async Task<IActionResult> Index()
        {
            //var courseService = new CourseService();
            //var courseService = new CourseService();
            List<CourseViewModel> courses = await courseService.GetCoursesAsync();
            ViewData["Title"] = "Catalogo dei Corsi";
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
            //return Content($"Sono detail, ho ricevuto l'id {id}");
            //var courseService = new CourseService();
            CourseDetailViewModel viewModel = await courseService.GetCourseAsync(id);
            ViewData["Title"] = viewModel.Title;
            return View(viewModel);
        }
        public IActionResult Search(string title)
        {
            return Content($"Hai cercato {title}");
        }


    }

}