using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required")
            .ForEach(item =>
            {
                item.ChildRules(itemRules =>
                {
                    itemRules.RuleFor(x => x.ItemId)
                        .NotEmpty()
                        .WithMessage("Item ID is required");

                    itemRules.RuleFor(x => x.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than 0")
                        .LessThanOrEqualTo(20)
                        .WithMessage("Maximum quantity per item is 20");
                });
            });
    }
} 