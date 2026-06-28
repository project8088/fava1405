using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Nikan.Common.WebToolkit
{
    public class CustomHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CustomHeadersToAddAndRemove _headers;


        public CustomHeadersMiddleware(RequestDelegate next, CustomHeadersToAddAndRemove headers)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            _next = next;
            _headers = headers;
        }

        public async Task Invoke(HttpContext context)
        {
            foreach (var headerValuePair in _headers.HeadersToAdd)
            {
                context.Response.Headers[headerValuePair.Key] = headerValuePair.Value;
            }
            foreach (var header in _headers.HeadersToRemove)
            {
                context.Response.Headers.Remove(header);
            }

            await _next(context);
        }
    }

    public class CustomHeadersToAddAndRemove
    {
        public Dictionary<string, string> HeadersToAdd = new Dictionary<string, string>();
        public HashSet<string> HeadersToRemove = new HashSet<string>();
        public bool PrimaryRequestsOnly { get; set; } = false;

        public List<string> PrimaryRequestMimeTypes { get; set; } = new List<string>()
        {
            "text/html",
            "application/json",
            "text/xml",
            "application/xml"
        };
    }
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Enable the Customer Headers middleware and specify the headers to add and remove.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="addHeadersAction">
        /// Action to allow you to specify the headers to add and remove.
        ///
        /// Example: (opt) =>  opt.HeadersToAdd.Add("header","value"); opt.HeadersToRemove.Add("header");</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomHeaders(this IApplicationBuilder builder, Action<CustomHeadersToAddAndRemove> addHeadersAction)
        {
            var headers = new CustomHeadersToAddAndRemove();
            addHeadersAction?.Invoke(headers);

            builder.UseMiddleware<CustomHeadersMiddleware>(headers);
            return builder;
        }
    }


}
 