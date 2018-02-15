using System;
using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApi.Hal.JsonConverters;

namespace WebApi.Hal
{
    internal class JsonHalFormattersMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly ArrayPool<char> _charPool;
        private readonly ObjectPoolProvider _objectPoolProvider;
        private readonly IServiceProvider _serviceProvider;

        public JsonHalFormattersMvcOptionsSetup(
            ILoggerFactory loggerFactory,
            IOptions<MvcJsonOptions> jsonOptions,
            ArrayPool<char> charPool,
            ObjectPoolProvider objectPoolProvider,
            IServiceProvider serviceProvider)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (jsonOptions == null)
            {
                throw new ArgumentNullException(nameof(jsonOptions));
            }

            if (charPool == null)
            {
                throw new ArgumentNullException(nameof(charPool));
            }

            if (objectPoolProvider == null)
            {
                throw new ArgumentNullException(nameof(objectPoolProvider));
            }

            _loggerFactory = loggerFactory;
            _jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
            _charPool = charPool;
            _objectPoolProvider = objectPoolProvider;
            _serviceProvider = serviceProvider;
        }

        public void Configure(MvcOptions options)
        {
            // Can't add these inside Input/Output formatters or end up with 2 copies
            _jsonSerializerSettings.Converters.Add(new LinksConverter());
            var resolver = (IHypermediaResolver)_serviceProvider.GetService(typeof(IHypermediaResolver));
            if (resolver == null)
            {
                _jsonSerializerSettings.Converters.Add(new ResourceConverter());
            }
            else
            {
                _jsonSerializerSettings.Converters.Add(new ResourceConverter(resolver));
            }

            _jsonSerializerSettings.Converters.Add(new EmbeddedResourceConverter());

            options.OutputFormatters.Add(new XmlHalMediaTypeOutputFormatter());
            options.OutputFormatters.Add(new JsonHalMediaTypeOutputFormatter(_jsonSerializerSettings, _charPool));

            // Register JsonHalMediaTypeInputFormatter before JsonInputFormatter, otherwise
            // JsonInputFormatter would consume "application/json-patch+json" requests
            // before JsonPatchInputFormatter gets to see them.
            var jsonInputPatchLogger = _loggerFactory.CreateLogger<JsonHalMediaTypeInputFormatter>();
            options.InputFormatters.Add(new JsonHalMediaTypeInputFormatter(
                jsonInputPatchLogger,
                _jsonSerializerSettings,
                _charPool,
                _objectPoolProvider));
        }
    }
}