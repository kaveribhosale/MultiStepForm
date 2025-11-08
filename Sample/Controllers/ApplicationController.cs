using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Sample.Models;

namespace Sample.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplicationController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var app = _context.AccountApplications.Find(id);
                return View(app);
            }
            return View(new AccountApplication());
        }

        [HttpPost]
        public async Task<IActionResult> SaveDraft(AccountApplication model, IFormFile? document)
        {
            if (document != null)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadPath);

                string fileName = $"{Guid.NewGuid()}_{document.FileName}";
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }

                model.DocumentPath = "/uploads/" + fileName;
            }

            if (model.Id == 0)
            {
                model.Status = "Draft";
                _context.AccountApplications.Add(model);
            }
            else
            {
                _context.AccountApplications.Update(model);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, id = model.Id });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitApplication(AccountApplication model)
        {
            AccountApplication existing;

            if (model.Id == 0)
            {
                model.Status = "Submitted";
                _context.AccountApplications.Add(model);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                existing = model;
            }
            else
            {
                existing = await _context.AccountApplications.FindAsync(model.Id);
                if (existing == null)
                    return Json(new { success = false, message = "Application not found." });

                _context.Entry(existing).CurrentValues.SetValues(model);
                existing.Status = "Submitted";
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, id = existing.Id });
        }


        public async Task<IActionResult> Summary(int id)
        {
            var app = await _context.AccountApplications.FindAsync(id);
            if (app == null) return NotFound();
            return View(app);
        }
        public async Task<IActionResult> ReviewAndSubmit(int id)
        {
            var app = await _context.AccountApplications.FindAsync(id);
            if (app == null)
                return RedirectToAction("Index");

            return View(app);
        }
    }
}
