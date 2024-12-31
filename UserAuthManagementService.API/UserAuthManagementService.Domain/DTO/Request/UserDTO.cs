using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthManagementService.Domain.DTO.Request;
    public class UserDto
    {

    public string name { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string role { get; set; }
    public string phoneNumber { get; set; }
    public string address { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string country { get; set; }
    public Double balance { get; set; }
    public string Password { get; set; }
}
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("name is required")
                .Length(3, 500).WithMessage("name must be between 3 and 500 characters.");
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("email is required")
                 .EmailAddress().WithMessage("Invalid email format.")
                .Length(10, 200).WithMessage("email must be between 3 and 500 characters.");
            RuleFor(x => x.phoneNumber)
        .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");
        RuleFor(x => x.address)
                .NotEmpty().WithMessage("address is required")
                .Length(5, 500).WithMessage("address must be between 5 and 500 characters.");
        RuleFor(x => x.city)
                .NotEmpty().WithMessage("city is required")
                .Length(5, 50).WithMessage("city must be between 5 and 50 characters.");
        RuleFor(x => x.country)
                .NotEmpty().WithMessage("country is required")
                .Length(5, 50).WithMessage("country must be between 5 and 50 characters.");
        RuleFor(x => x.state)
                .NotEmpty().WithMessage("state is required")
                .Length(5, 50).WithMessage("state must be between 5 and 50 characters.");
            RuleFor(x => x.balance)
                .NotEmpty().WithMessage("price is required");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(10, 13).WithMessage("Password must be between 10 and 13 characters for ISBN-10, ISBN-13 respectively.");
        }
    }


