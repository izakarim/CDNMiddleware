using System;
using System.Net;
using CDNMiddleware.DataAccess.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CDNMiddleware.DataAccess.Extensions
{
	public static class ActionContextExtension
	{
        public static ApiResponse CustomizeErrorResponse(this ActionContext actionContext)
        {
            ApiResponse response = new ApiResponse();

            var enumerable = actionContext
                                .ModelState
                                .Where(modelError =>
                                    modelError.Value.Errors.Count > 0
                                );

            var firstColumn = enumerable
                                .FirstOrDefault();

            response.Code = HttpStatusCode.UnprocessableEntity;

            response.Message = firstColumn.Key.Equals("id") ? "Record not found." : firstColumn
                                .Value.Errors
                                .FirstOrDefault()
                                .ErrorMessage;

            response.Errors = actionContext
                                .ModelState
                                .Where(modelError => modelError.Value.Errors.Count > 0)
                                .Select(modelError => new Error()
                                {
                                    errorCode = modelError.Key,
                                    message = modelError.Value.Errors.FirstOrDefault().ErrorMessage
                                })
                                .ToList();

            return response;
        }
    }
}

