using Microsoft.AspNetCore.Mvc;
using MovieLab.Models;
using Microsoft.EntityFrameworkCore;
using MovieLab.ViewModels;
namespace MovieLab.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieFormViewModel
            {
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };  
            return View("MovieForm", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model) // the stuff u get from the form
        {

            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select a file");
                return View("MovieForm", model);

            }
            var poster= files.FirstOrDefault();
            var Allowedextensions = new List<string> { ".jpg", ".jpeg", ".png" };
            if (!Allowedextensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select a file with .jpg, .jpeg, or .png format only");
                return View("MovieForm", model);
            }
            using var DataStream = new MemoryStream();
            await poster.CopyToAsync(DataStream);
            var movie = new Movie
            {
                Title = model.Title,
                Year = model.Year,
                Rate = model.Rate,
                Story = model.Story,
                Poster = DataStream.ToArray(),
                GenreId = model.GenreId
            };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {   if (id == null) return BadRequest();
        var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            string posterBase64 = null;
            if (movie.Poster != null)
            {
                posterBase64 = Convert.ToBase64String(movie.Poster);
            }

            var model = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Rate = movie.Rate,
                Story = movie.Story,
                PosterBase64 = posterBase64,
                GenreId = movie.GenreId,
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };
            return View("MovieForm", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            
            var movie = await _context.Movies.FindAsync(model.Id);
           
            movie.Title= model.Title;
            movie.Year= model.Year;
            movie.Rate= model.Rate;
            movie.Story= model.Story;
            if (model.Poster != null)
            {
                var files = Request.Form.Files;
                var poster= files.FirstOrDefault();
                var Allowedextensions = new List<string> { ".jpg", ".jpeg", ".png" };
                if (!Allowedextensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Please select a file with .jpg, .jpeg, or .png format only");
                    return View("MovieForm", model);
                }
                using var DataStream = new MemoryStream();
                await poster.CopyToAsync(DataStream);
                movie.Poster = DataStream.ToArray();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}
