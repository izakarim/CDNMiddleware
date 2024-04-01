using System;
using CDNMiddleware.DataAccess.Models;
using CDNMiddleware.DataAccess.ViewModels;

namespace CDNMiddleware.Application.Services.Interfaces
{
	public interface IUserService
	{
        (bool status, string message, object? data) List();
        (bool status, string message, object? data) Create(UserViewModel viewModel);
        (bool status, string message, object? data) Update(Int64 id, UserViewModel viewModel);
        (bool status, string message, object? data) Delete(Int64 id);
    }
}

