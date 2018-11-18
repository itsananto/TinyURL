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
using TinyURL.Services;
using TinyURL.Utilities;
using TinyURL.ViewModels;

namespace TinyURL.Controllers
{
    public class URLMapsController : Controller
    {
        private readonly IMapper _mapper;
        private IURLMapsService _service;

        public URLMapsController(IMapper mapper, IURLMapsService service)
        {
            _mapper = mapper;
            _service = service;
        }

        // GET: URLMaps
        public IActionResult Index(string encoded)
        {

            if (encoded != null)
            {
                if (_service.EncodedURLMapExists(encoded))
                {
                    return Redirect(_service.GetURLBasedOnEncodedURL(encoded));
                }
                else
                {
                    return View("Error");
                }
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
                uRLMap.URL = Helper.ValidateURL(uRLMap.URL);

                string encodedPart = "";
                if (_service.URLMapExists(uRLMap.URL))
                {
                    encodedPart = _service.GetEncodedURLBasedOnURL(uRLMap.URL);
                }
                else
                {
                    var map = _mapper.Map<URLMapViewModel, URLMap>(uRLMap);
                    map.EncodedURL = Helper.GenerateShortURL();

                    _service.SaveURLMap(map);

                    encodedPart = map.EncodedURL;
                }

                TempData["encoded"] = Request.Scheme + Uri.SchemeDelimiter + Request.Host + "/" + encodedPart;
                TempData["url"] = uRLMap.URL;


                return RedirectToAction(nameof(Index));
            }
            return View(uRLMap);
        }

        
    }
}
