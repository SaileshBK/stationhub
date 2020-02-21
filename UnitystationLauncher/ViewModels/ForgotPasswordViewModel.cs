﻿using System;
using System.Reactive;
using UnitystationLauncher.Models;
using ReactiveUI;

namespace UnitystationLauncher.ViewModels
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        private readonly Lazy<LoginViewModel> loginVM;
        private readonly AuthManager authManager;
        private bool isFormVisible;
        string? email;
        public string? Email
        {
            get => email;
            set => this.RaiseAndSetIfChanged(ref email, value);
        }

        public ReactiveCommand<Unit, Unit> Submit { get; }
        public ReactiveCommand<Unit, LoginViewModel?> DoneButton { get; }

        public bool IsFormVisible
        {
            get => isFormVisible;
            set => this.RaiseAndSetIfChanged(ref isFormVisible, value);
        }

        public ForgotPasswordViewModel(AuthManager authManager, Lazy<LoginViewModel> loginVM)
        {
            IsFormVisible = true;
            this.authManager = authManager;
            this.loginVM = loginVM;
            
            var inputValidation = this.WhenAnyValue(
                x => x.Email,
                (e) => !string.IsNullOrWhiteSpace(e) &&
                e.Contains("@") && e.Contains("."));

            Submit = ReactiveCommand.Create(
                TrySendResetPassword, inputValidation);

            DoneButton = ReactiveCommand.Create(ReturnToLogin);
        }

        void TrySendResetPassword()
        {
            authManager.SendForgotPasswordEmail(Email);
        }

        public LoginViewModel? ReturnToLogin()
        {
            return loginVM.Value;
        }
    }
}
