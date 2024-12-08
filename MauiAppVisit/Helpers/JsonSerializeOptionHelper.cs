using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppVisit.Helpers
{
    public static class JsonSerializeOptionHelper
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static JsonSerializerOptions Options
        {
            get { return _options; }
        }
    }
}
