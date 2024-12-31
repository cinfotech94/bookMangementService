using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPurchaseManagementService.Domain.DTO.Request;
    public class BookDTO
    {
    public Guid id { get; set; }

    public string title { get; set; }
    public string ISBN { get; set; }
    public string author { get; set; }
    public string publicationYear { get; set; }
    public DateTime timeAdded { get; private set; } = DateTime.Now;
    public string genre { get; set; }
    public int quantity { get; set; } = 1;
    public double price { get; set; }
    public int? pages { get; set; }
    public string description { get; set; }
    public string category { get; set; }
    public int? noClick { get; set; }
    public int? noOfPPurchase { get; set; }
    public int? noOfCart { get; set; }
}
    public class BookDtoValidator : AbstractValidator<BookDTO>
    {
        public BookDtoValidator()
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("title is required")
                .Length(3, 500).WithMessage("title must be between 3 and 500 characters.");
            RuleFor(x => x.author)
                .NotEmpty().WithMessage("author is required")
                .Length(2, 100).WithMessage("author must be between 3 and 500 characters.");
            RuleFor(x => x.genre)
                .NotEmpty().WithMessage("genre is required")
                .Length(3, 500).WithMessage("genre must be between 3 and 500 characters.");
            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required")
                .Length(10, 13).WithMessage("ISBN must be between 10 and 13 characters for ISBN-10, ISBN-13 respectively.");
            RuleFor(x => x.price)
                .NotEmpty().WithMessage("price is required");
            RuleFor(x => x.publicationYear)
                .NotEmpty().WithMessage("publicationYear is required")
                .Length(10, 13).WithMessage("publicationYear must be between 10 and 13 characters for ISBN-10, ISBN-13 respectively.");
            RuleFor(x => x.timeAdded)
                .NotEmpty().WithMessage("timeAdded is required")
                .Must(BeAValidDate).WithMessage("timeAdded must be between 3 and 500 characters.");
            RuleFor(x => x.quantity)
                .NotEmpty().WithMessage("quantity is required");
            RuleFor(x => x.category)
                .NotEmpty().WithMessage("category is required");
        }
        private bool BeAValidDate(DateTime timeAdded)
        {
            return timeAdded >= DateTime.Now.AddDays(-30) && timeAdded <= DateTime.Now;
        }
    }


