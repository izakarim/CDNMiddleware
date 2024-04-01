using System;
using CDNMiddleware.DataAccess.Constants;
using CDNMiddleware.DataAccess.Helpers;
using CDNMiddleware.Application;
using Microsoft.AspNetCore.Mvc;
using CDNMiddleware.Application.Services.Interfaces;
using CDNMiddleware.DataAccess.ViewModels;

namespace CDNMiddleware.Api.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Route("list")]
        public IActionResult List()
        {
            ApiResponse response = new();

            try
            {
                (response.Status, response.Message, response.Data) = _userService.List();
            }
            catch (Exception ex)
            {
                response.setMessage(ex.Message)
                        .setErrors(new()
                        {
                            errorCode = CodeConstant.Exception,
                            message = ex.Message ?? $"An exception has occurred. \n{nameof(ex)}",
                        });
            }

            return response.CodeAsResult();
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] UserViewModel viewModel)
        {
            ApiResponse response = new();

            try
            {
                (response.Status, response.Message, response.Data) = _userService.Create(viewModel);
            }
            catch (Exception ex)
            {
                response.setMessage(ex.Message)
                        .setErrors(new()
                        {
                            errorCode = CodeConstant.Exception,
                            message = ex.Message ?? $"An exception has occurred. \n{nameof(ex)}",
                        });
            }

            return response.CodeAsResult();
        }

        [HttpPut("update/{id}")]
        public ActionResult Update(Int64 id, [FromBody] UserViewModel viewModel)
        {
            ApiResponse response = new();

            try
            {
                (response.Status, response.Message, response.Data) = _userService.Update(id, viewModel);
            }
            catch (Exception ex)
            {
                response.setMessage(ex.Message)
                        .setErrors(new()
                        {
                            errorCode = CodeConstant.Exception,
                            message = ex.Message ?? $"An exception has occurred. \n{nameof(ex)}",
                        });
            }

            return response.CodeAsResult();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(Int64 id)
        {
            ApiResponse response = new();

            try
            {
                (response.Status, response.Message, response.Data) = _userService.Delete(id);
            }
            catch (Exception ex)
            {
                response.setMessage(ex.Message)
                        .setErrors(new()
                        {
                            errorCode = CodeConstant.Exception,
                            message = ex.Message ?? $"An exception has occurred. \n{nameof(ex)}",
                        });
            }

            return response.CodeAsResult();
        }
    }
}

