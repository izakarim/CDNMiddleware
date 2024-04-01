using System;
using Microsoft.AspNetCore.Http.Headers;

namespace CDNMiddleware.DataAccess.Extensions
{
    public static class RequestHeadersExtension
    {
        public static bool AcceptHeadersIs(this RequestHeaders headers, string type)
        {
            return headers.Accept.Any(header => header.MediaType == type);
        }

        public static bool AcceptHeaderIsJson(this RequestHeaders headers)
        {
            return headers.AcceptHeadersIs("application/json");
        }
    }
}

