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
        public IActionResult Index(string encoded)
        {

            if (encoded != null && URLMapExists(encoded))
            {
                ViewBag.EncodedURL = Request.Scheme + Uri.SchemeDelimiter + Request.Host + "/" + encoded;
                ViewBag.URL = _context.URLMap.FirstOrDefault(x => x.EncodedURL == encoded).URL;
            }
            else
            {
                ViewBag.EncodedURL = TempData.ContainsKey("encoded") ? TempData["encoded"] : null;
                ViewBag.URL = TempData.ContainsKey("url") ? TempData["url"] : null;
            }

            return View(new URLMapViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("URL")] URLMapViewModel uRLMap)
        {
            if (ModelState.IsValid)
            {
                string encodedPart = "";
                if (URLMapExists(new Uri(uRLMap.URL)))
                {
                    encodedPart = _context.URLMap.FirstOrDefault(x=>x.URL == uRLMap.URL).EncodedURL;
                }
                else
                {
                    var map = _mapper.Map<URLMapViewModel, URLMap>(uRLMap);
                    map.EncodedURL = GenerateShortURL();

                    _context.Add(map);
                    await _context.SaveChangesAsync();

                    encodedPart = map.EncodedURL;
                }

                TempData["encoded"] = Request.Scheme + Uri.SchemeDelimiter + Request.Host + "/" + encodedPart;
                TempData["url"] = uRLMap.URL;


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

        private bool URLMapExists(string encodedURL)
        {
            return _context.URLMap.Any(e => e.EncodedURL == encodedURL);
        }

        private bool URLMapExists(Uri URL)
        {
            return _context.URLMap.Any(e => e.URL == URL.OriginalString);
        }
    }
}
