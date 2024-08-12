using Microsoft.AspNetCore.Mvc;
using MovieLab.Models;
using Microsoft.EntityFrameworkCore;
using MovieLab.ViewModels;
using NToastNotify;
namespace MovieLab.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public MoviesController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            List<Movie> movies = await _context.Movies.OrderByDescending(m => m.Rate).ToListAsync();
            return View(movies);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel= new MovieFormViewModel
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
            IFormFileCollection files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select a file");
                return View("MovieForm", model);

            }
            IFormFile? poster = files.FirstOrDefault();
            List<string> Allowedextensions = [".jpg", ".jpeg", ".png"];
            if (!Allowedextensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select a file with .jpg, .jpeg, or .png format only");
                return View("MovieForm", model);
            }
            using var DataStream = new MemoryStream(); ;
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
            _ = _context.Movies.Add(movie);
            _ = await _context.SaveChangesAsync();

            _toastNotification.AddSuccessToastMessage("Movie created successfully");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Movie? movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            string? posterBase64 = null;
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

            Movie? movie = await _context.Movies.FindAsync(model.Id);

            movie.Title = model.Title;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.Story = model.Story;
            IFormFileCollection files = Request.Form.Files;
            if (files.Any())
            {
                IFormFile? poster = files.FirstOrDefault();
                List<string> Allowedextensions = [".jpg", ".jpeg", ".png"];
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
            _toastNotification.AddSuccessToastMessage("Movie Edited successfully");
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Movie? movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            string? posterBase64 = null;
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
            return View(model);
        }
        [Route("movies/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Movie? movie = await _context.Movies.FindAsync(id);


            if (movie == null)
            {
                return NotFound();
            }
            _ = _context.Movies.Remove(movie);
            _ = await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("Movie Deleted successfully");

            return NoContent();
        }


    }

}
