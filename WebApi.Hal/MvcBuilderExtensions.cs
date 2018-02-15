using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace WebApi.Hal
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddJsonHalFormatters(this IMvcCoreBuilder builder)
        {
            return AddJsonHalFormatters(builder, null);
        }

        public static IMvcCoreBuilder AddJsonHalFormatters(this IMvcCoreBuilder builder, IHypermediaResolver hypermediaResolver)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (hypermediaResolver != null)
            {
                builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(x => hypermediaResolver));
            }

            ServiceDescriptor descriptor = ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonHalFormattersMvcOptionsSetup>();
            builder.Services.TryAddEnumerable(descriptor);

            return builder;
        }

        public static IMvcBuilder AddJsonHalFormatters(this IMvcBuilder builder)
        {
            return AddJsonHalFormatters(builder, null);
        }

        public static IMvcBuilder AddJsonHalFormatters(this IMvcBuilder builder, IHypermediaResolver hypermediaResolver)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (hypermediaResolver != null)
            {
                builder.Services.TryAddEnumerable(ServiceDescriptor.Transient(x => hypermediaResolver));
            }

            ServiceDescriptor descriptor = ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, JsonHalFormattersMvcOptionsSetup>();
            builder.Services.TryAddEnumerable(descriptor);

            return builder;
        }
    }
}