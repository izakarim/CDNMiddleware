using System;
using CDNMiddleware.Application.DataAccess.Interfaces;
using CDNMiddleware.Application.Services.Interfaces;
using CDNMiddleware.DataAccess.Helpers;
using CDNMiddleware.DataAccess.Models;
using CDNMiddleware.DataAccess.ViewModels;

namespace CDNMiddleware.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserDataAccess _userDataAccess;

		public UserService(IUserDataAccess userDataAccess)
		{
            _userDataAccess = userDataAccess;
		}

        public (bool status, string message, object? data) List()
        {
            bool status = false;
            string message = "No records found.";
            object? data = null;

            try
            {
                List<User>? users = _userDataAccess.GetUsers();

                if (users is null) return (status, message, data);

                status = true;
                message = $"{users.Count} found.";
                data = users;
            }
            catch (Exception ex)
            {
                message = ex.Message ?? message;
            }

            return (status, message, data);
        }

        public (bool status, string message, object? data) Create(UserViewModel viewModel)
        {
            bool status = false;
            string message = "Failed to create user.";
            object? data = null;

            try
            {
                User? user = new User()
                {
                    Username = viewModel.Username,
                    Mail = viewModel.Mail,
                    PhoneNumber = GlobalFunction.EncryptString(viewModel.PhoneNumber),
                    Skillsets = viewModel.Skillsets,
                    Hobby = viewModel.Hobby
                };

                user = _userDataAccess.Store(user);

                if (user is null) return (status, message, data);

                status = true;
                message = "User registered successfully.";
                data = user;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message ?? message;
            }

            return (status, message, data);
        }

        public (bool status, string message, object? data) Update(Int64 id, UserViewModel viewModel)
        {
            bool status = false;
            string message = "Failed to update user.";
            object? data = null;

            try
            {
                User? user = _userDataAccess.FindUserById(id);

                if (user is null) return (status, message, data);

                user.Username = viewModel.Username;
                user.Mail = viewModel.Mail;
                user.PhoneNumber = GlobalFunction.EncryptString(viewModel.PhoneNumber);
                user.Skillsets = viewModel.Skillsets;
                user.Hobby = viewModel.Hobby;

                user = _userDataAccess.Update(user);

                status = true;
                message = "User updated successfully.";
                data = user;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message ?? message;
            }

            return (status, message, data);
        }

        public (bool status, string message, object? data) Delete(Int64 id)
        {
            bool status = false;
            string message = "Failed to delete.";
            object? data = null;

            try
            {
                User? user = _userDataAccess.FindUserById(id);

                if (user is null) return (status, "User not found.", data);

                _userDataAccess.Delete(user);

                return (true, "Successfully delete user.", data);
            }
            catch (Exception ex)
            {
                message = ex.Message ?? message;
            }

            return (status, message, data);
        }
    }
}

