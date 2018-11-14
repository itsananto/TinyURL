using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyURL.Models
{
    public class URLMap
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string EncodedURL { get; set; }
    }
}
