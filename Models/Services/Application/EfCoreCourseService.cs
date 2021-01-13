using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MyCourse.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    {
        private readonly MyCourseDbContext dbContext;

        public EfCoreCourseService(MyCourseDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
            CourseDetailViewModel viewModel = await dbContext.Courses
                .Where(course => course.Id == id)
                .Select(course =>
                new CourseDetailViewModel {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    CurrentPrice = course.CurrentPrice,
                    FullPrice = course.FullPrice,
                    Lessons = course.Lessons.Select(lesson => new LessonViewModel 
                    {
                        Id = lesson.Id,
                        Title = lesson.Title,
                        Description = lesson.Description,
                        Duration = lesson.Duration
                    }).ToList()
                })
                .SingleAsync(); //Restituisce il primo elemento dell'elenco. in caso di 0 o +1 solleva eccezione
                //.FirstAsync() -- restitusce sempre il primo elemento, ma da errore se elenco vuoto
                //.SingleOrDefaultAsync tollera che l'elenco sia vuoto, ma non piu elementi di 1.
                //.FirstOrDefaultAsync non solleva mai eccezione, restituisce il primo se c'è altriment null se è vuoto    
            return viewModel;
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync()
        {
            List<CourseViewModel> courses = await dbContext.Courses.Select(course => 
            new CourseViewModel {
                Id = course.Id,
                Title = course.Title,
                ImagePath = course.ImagePath,
                Author = course.Author,
                Rating = course.Rating,
                CurrentPrice = course.CurrentPrice,
                FullPrice = course.FullPrice
            })
            .ToListAsync();

            return courses;
        }
    }
}