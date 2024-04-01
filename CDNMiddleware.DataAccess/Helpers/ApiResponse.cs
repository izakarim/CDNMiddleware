using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using CDNMiddleware.DataAccess.Constants;
using Microsoft.AspNetCore.Mvc;

namespace CDNMiddleware.DataAccess.Helpers
{
    public class ApiResponse
    {
        [Newtonsoft.Json.JsonProperty("success"), JsonPropertyName("success")]
        public bool Status { get; set; } = false;

        [Newtonsoft.Json.JsonProperty("message"), JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [Newtonsoft.Json.JsonProperty("errors"), JsonPropertyName("errors")]
        public List<Error> Errors { get; set; } = new List<Error>();

        [Newtonsoft.Json.JsonProperty("data"), JsonPropertyName("data")]
        public object? Data { get; set; } = null;

        [IgnoreDataMember, JsonIgnore]
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;

        public ApiResponse setFailure()
        {
            this.Status = false;
            return this;
        }

        public ApiResponse setSuccess()
        {
            this.Status = true;
            return this;
        }

        public ApiResponse setCode(HttpStatusCode code)
        {
            this.Code = code;
            return this;
        }

        public ApiResponse setMessage(string message)
        {
            this.Message = message;
            return this;
        }

        public ApiResponse setData(object data)
        {
            this.Data = data;
            return this;
        }

        public ApiResponse setErrors(Error errors)
        {
            this.Errors.Add(errors);
            return this;
        }

        public bool isSuccess()
        {
            return Status;
        }
        public bool isFailure()
        {
            return !Status;
        }

        public IActionResult asResult()
        {
            return new OkObjectResult(this);
        }

        public ActionResult CodeAsResult()
        {
            switch (Code)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.MethodNotAllowed:
                    return new OkObjectResult(this);
                case HttpStatusCode.NoContent:
                    return new NoContentResult();
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(this);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(this);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(this);
                default:
                    return new NotFoundObjectResult(this);
            }
        }

        public static ApiResponse byStatusCode(int statusCode)
        {
            ApiResponse response = new ApiResponse()
            {
                Code = HttpStatusCode.InternalServerError,
                Message = "Something went wrong. Please contact administrator.",
                Errors = new List<Error>() { }
            };

            if (statusCode == (int)HttpStatusCode.NotFound)
            {
                response.Code = HttpStatusCode.NotFound;
                response.Message = "Page not found";
                response.Errors.Add(new Error()
                {
                    errorCode = CodeConstant.Page.NotFound,
                    message = "Page not found",
                });
            }
            else if (statusCode == (int)HttpStatusCode.Forbidden)
            {
                response.Code = HttpStatusCode.Forbidden;
                response.Message = "Permission denied. You are not allowed to access this page.";
                response.Errors.Add(new Error()
                {
                    errorCode = CodeConstant.Page.Forbidden,
                    message = "Permission denied. You are not allowed to access this page.",
                });
            }
            else if (statusCode == (int)HttpStatusCode.MethodNotAllowed)
            {
                response.Code = HttpStatusCode.MethodNotAllowed;
                response.Message = "This method is not allowed to access this page.";
                response.Errors.Add(new Error()
                {
                    errorCode = CodeConstant.Page.MethodNotAllowed,
                    message = "This method is not allowed to access this page.",
                });
            }

            return response;
        }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.asResult());
        }
    }

    public class Error
    {
        public string errorCode { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }
}

