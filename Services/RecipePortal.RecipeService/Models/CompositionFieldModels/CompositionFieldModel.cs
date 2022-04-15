using AutoMapper;
using RecipePortal.Db.Entities;

namespace RecipePortal.RecipeService.Models;

public class CompositionFieldModel
{
    public int CompositionFieldId { get; set; }
    public string IngredientId { get; set; }
    public string Quantity { get; set; }
}

public class CompositionFeildResponseProfile : Profile
{
    public CompositionFeildResponseProfile()
    {
        CreateMap<CompositionField, CompositionFieldModel>()
            .ForMember(d => d.CompositionFieldId,a => a.MapFrom(src => src.Id));
    }
}
