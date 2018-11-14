using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinyURL.Models;
using TinyURL.ViewModels;

namespace TinyURL.Controllers
{
    public class URLMapsController : Controller
    {
        private readonly TinyURLContext _context;

        public URLMapsController(TinyURLContext context)
        {
            _context = context;
        }

        // GET: URLMaps
        public IActionResult Index()
        {
            return View(new URLMapViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("URL")] URLMapViewModel uRLMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uRLMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uRLMap);
        }

        public string GenerateShortURL(string url)
        {
            throw new NotImplementedException();
        }

        // GET: URLMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uRLMap = await _context.URLMap
                .SingleOrDefaultAsync(m => m.ID == id);
            if (uRLMap == null)
            {
                return NotFound();
            }

            return View(uRLMap);
        }

        // POST: URLMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uRLMap = await _context.URLMap.SingleOrDefaultAsync(m => m.ID == id);
            _context.URLMap.Remove(uRLMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool URLMapExists(int id)
        {
            return _context.URLMap.Any(e => e.ID == id);
        }
    }
}
