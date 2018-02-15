using System;
using System.Buffers;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using WebApi.Hal.JsonConverters;

namespace WebApi.Hal
{
    public class JsonHalMediaTypeInputFormatter : JsonInputFormatter
    {
        private const string _mediaTypeHeaderValueName = "application/hal+json";

        public JsonHalMediaTypeInputFormatter(
            ILogger logger, JsonSerializerSettings serializerSettings, ArrayPool<char> charPool, ObjectPoolProvider objectPoolProvider) : base(logger, serializerSettings, charPool, objectPoolProvider)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(_mediaTypeHeaderValueName));
        }

        protected override JsonSerializer CreateJsonSerializer()
        {
            var serializer = base.CreateJsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Include;
            return serializer;
        }
    }
}