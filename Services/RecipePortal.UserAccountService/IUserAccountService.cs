using RecipePortal.UserAccountService.Models;

namespace RecipePortal.UserAccountService;

public interface IUserAccountService
{
    Task<UserAccountModel> Create (RegisterUserAccountModel model);


    //SUBSCRIBE
    Task AddSubscriptionToAuthor(Guid subscriber, string autorNickname);

    Task AddSubscriptionToCategory(SubscriptionToCategoryModel model);

    Task AddSubscriptionToComments(SubscriptionToCommentsModel model);


    // .. Также здесь можно разместить методы для изменения данных учетной записи, восстановления и смены пароля, подтверждения электронной почты, установки телефона и его подтверждения и т.д.
    // .. Но это уже на самостоятельно.
    // .. Удачи! Я в вас верю!  :)
}
