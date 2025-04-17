using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product Name is required.")
                .MaximumLength(255).WithMessage("Product Name must be 255 characters or fewer.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("ModifiedBy is required.")
                .MaximumLength(100).WithMessage("ModifiedBy must be 100 characters or fewer.");
        }
    }
}
