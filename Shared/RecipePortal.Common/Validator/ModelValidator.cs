using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipePortal.Common.Validator;
public class ModelValidator<T> : IModelValidator<T> where T : class
{
    private readonly IValidator<T> validator;

    public ModelValidator(IValidator<T> validator)
    {
        this.validator = validator;
    }

    public void Check(T model)
    {
        var result = validator.Validate(model);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
