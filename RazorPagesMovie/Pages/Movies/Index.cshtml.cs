using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Movie != null)
            {
                Movie = await _context.Movie.ToListAsync();
            }
        }

        [TempData]
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile fileImport)
        {
            if (fileImport == null || fileImport.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một tệp.");
                return Page();
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await fileImport.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            var movie = new Movie
                            {
                                Title = worksheet.Cells[row, 1].Value.ToString(),
                                ReleaseDate = DateTime.Parse(worksheet.Cells[row, 2].Value.ToString()),
                                Genre = worksheet.Cells[row, 3].Value.ToString(),
                                Price = decimal.Parse(worksheet.Cells[row, 4].Value.ToString())
                            };

                            _context.Movie.Add(movie);
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                SuccessMessage = "Dữ liệu đã được import thành công.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình import dữ liệu: " + ex.Message);
                return Page();
            }

            return RedirectToPage("/Movies/Index");
        }
    }
}
