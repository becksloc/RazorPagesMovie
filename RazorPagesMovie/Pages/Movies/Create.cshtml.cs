using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesMovie.Data;
using RazorPagesMovie.Migrations;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public CreateModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {            
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        [BindProperty]
        [Display(Name = "Chọn ảnh minh họa")]
        public IFormFile? UploadedFile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Movie == null || Movie == null)
            {
                return Page();
            }

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                string fileName = UploadedFile.FileName;
                string currentDir = Directory.GetCurrentDirectory();
                string filePath = currentDir + "\\wwwroot\\upload\\" + fileName;

                using (var stream = System.IO.File.Create(filePath))
                {
                    await UploadedFile.CopyToAsync(stream);
                    Movie.ImageUrl = fileName;
                }
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
