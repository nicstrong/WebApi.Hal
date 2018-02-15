using System;
using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace WebApi.Hal
{
    public class JsonHalMediaTypeOutputFormatter : JsonOutputFormatter
    {
        private const string _mediaTypeHeaderValueName = "application/hal+json";

       public JsonHalMediaTypeOutputFormatter(
            JsonSerializerSettings serializerSettings, 
            ArrayPool<char> charPool) :
            base(serializerSettings, charPool)
        {
            Initialize();
        }

        private void Initialize()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(_mediaTypeHeaderValueName));
        }

        protected override JsonSerializer CreateJsonSerializer()
        {
            var serializer = base.CreateJsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            // Can't be Error or the work around used in ResourceConverter.WriteJson() to remove itself from the
            // list of converters and re-call Serialize will fail as JSON.net will already have marked the object
            // as serialized
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;

            return serializer;
        }

        protected override bool CanWriteType(Type type)
        {
            return typeof(Representation).IsAssignableFrom(type);
        }
    }
}
