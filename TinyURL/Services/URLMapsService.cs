using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyURL.Models;

namespace TinyURL.Services
{
    public class URLMapsService : IURLMapsService
    {
        private readonly TinyURLContext _context;

        public URLMapsService(TinyURLContext context)
        {
            _context = context;
        }
        
        public bool EncodedURLMapExists(string encodedURL)
        {
            return _context.URLMap.Any(e => e.EncodedURL == encodedURL);
        }

        public bool URLMapExists(string URL)
        {
            return _context.URLMap.Any(e => e.URL == URL);
        }

        public string GetURLBasedOnEncodedURL(string encoded)
        {
            return _context.URLMap.FirstOrDefault(x => x.EncodedURL == encoded).URL;
        }

        public string GetEncodedURLBasedOnURL(string URL)
        {
            return _context.URLMap.FirstOrDefault(x => x.URL == URL).EncodedURL;
        }

        public void SaveURLMap(URLMap map)
        {
            _context.Add(map);
            _context.SaveChanges();
        }
    }
}
