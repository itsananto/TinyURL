using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public URLMapsController(TinyURLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: URLMaps
        public IActionResult Index()
        {
            GenerateShortURL();
            return View(new URLMapViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("URL")] URLMapViewModel uRLMap)
        {
            if (ModelState.IsValid)
            {

                var map = _mapper.Map<URLMapViewModel, URLMap>(uRLMap);

                //_context.Add(uRLMap);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uRLMap);
        }

        public string GenerateShortURL()
        {
            StringBuilder encodedURL = new StringBuilder();
            string reference = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Random rnd = new Random();
            for (int i = 0; i < 7; i++)
            {
                int rand = rnd.Next(0, reference.Length - 1);
                encodedURL.Append(reference[rand]);
            }

            return encodedURL.ToString();
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
