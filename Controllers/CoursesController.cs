using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            var courseService = new CourseService();
            //var courseService = new CourseService();
            List<CourseViewModel> courses = courseService.GetCourses();
            ViewData["Title"] = "Catalogo dei Corsi";
            return View(courses);
        }
        public IActionResult Detail(int id)
        {
            //return Content($"Sono detail, ho ricevuto l'id {id}");
            var courseService = new CourseService();
            CourseDetailViewModel viewModel = courseService.GetCourse(id);
            ViewData["Title"] = viewModel.Title;
            return View(viewModel);
        }
        public IActionResult Search(string  title)
        {
            return Content($"Hai cercato {title}");
        }


    }

}