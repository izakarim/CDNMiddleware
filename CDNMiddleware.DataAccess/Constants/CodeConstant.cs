using System;
namespace CDNMiddleware.DataAccess.Constants
{
	public class CodeConstant
	{
        public const string Exception = "THROW_EXCEPTION";

        public class Page
        {
            public const string NotFound = "PAGE_NOT_FOUND";
            public const string MethodNotAllowed = "METHOD_NOT_ALLOWED";
            public const string Forbidden = "FORBIDDEN";
            public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        }
    }
}

