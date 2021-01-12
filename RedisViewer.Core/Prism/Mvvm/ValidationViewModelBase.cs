using FluentValidation;
using FluentValidation.Results;
using System;
using System.ComponentModel;
using System.Linq;

namespace Prism.Mvvm
{
    public abstract class ValidatableBindableBase<T> : DialogViewModelBase, IDataErrorInfo
    {
        private IValidator<T> _validator;
        private T _instance;

        public ValidatableBindableBase()
        {

        }

        public void SetValidator(IValidator<T> validator, T instance)
        {
            _validator = validator;
            _instance = instance;
        }

        private ValidationResult SelfValidate()
        {
            return _validator.Validate(_instance);
        }

        private string GetValidationError(string propertyName = null)
        {
            var validationResult = SelfValidate();

            if (!validationResult.IsValid)
            {
                if (propertyName == null)
                    return string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                else
                    return validationResult.Errors.FirstOrDefault(e => e.PropertyName == propertyName)?.ErrorMessage ?? string.Empty;
            }

            return string.Empty;
        }

        public bool Validate()
        {
            IsValidationEnabled = true;
            return SelfValidate().IsValid;
        }

        public string Error
        {
            get => GetValidationError();
        }

        public string this[string propertyName]
        {
            get => GetValidationError(propertyName);
        }

        private bool _isValidationEnabled;
        public bool IsValidationEnabled
        {
            get => _isValidationEnabled;
            set => SetProperty(ref _isValidationEnabled, value);
        }
    }
}