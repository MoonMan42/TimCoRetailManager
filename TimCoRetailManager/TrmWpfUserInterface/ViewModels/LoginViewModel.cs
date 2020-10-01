﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;

namespace TrmWpfUserInterface.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private bool _isErrorVisible;
        private string _errorMessage;

        private IApiHelper _apiHelper;

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }



        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }



        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool result = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    return result = true;
                }

                return result;
            }
        }

        public async Task LogIn()
        {
            ErrorMessage = ""; // cleared out the error message on next try
            try
            {

                var result = await _apiHelper.Authenticate(UserName, Password);

                //Capture More info about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}
