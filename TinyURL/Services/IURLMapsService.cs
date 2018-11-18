using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyURL.Models;

namespace TinyURL.Services
{
    public interface IURLMapsService
    {
        bool EncodedURLMapExists(string encodedURL);
        bool URLMapExists(string URL);
        string GetURLBasedOnEncodedURL(string encoded);
        string GetEncodedURLBasedOnURL(string URL);
        void SaveURLMap(URLMap map);
    }
}
