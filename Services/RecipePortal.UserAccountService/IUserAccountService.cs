using RecipePortal.UserAccountService.Models;

namespace RecipePortal.UserAccountService;

public interface IUserAccountService
{
    Task<IEnumerable<UserAccountModel>> GetUsers(string authorNickname, int offset, int limit);
    Task<UserAccountModel> GetUser (string authorNickname);
    Task<UserAccountModel> Create (RegisterUserAccountModel model);


    //SUBSCRIPTIONS
    Task<SubscriptionToAuthorModel> AddSubscriptionToAuthor(Guid subscriber, string authorNickname);

    Task DeleteSubscriptionToAuthor(DeleteSubscriptionModel model);

    Task<SubscriptionToCategoryModel> AddSubscriptionToCategory(AddSubscriptionToCategoryModel model);

    Task DeleteSubscriptionToCategory(DeleteSubscriptionModel model);

    Task<SubscriptionToCommentsModel> AddSubscriptionToComments(AddSubscriptionToCommentsModel model);

    Task DeleteSubscriptionToComments(DeleteSubscriptionModel model);

    Task<AllSubscriptionsModel> GetSubscriptions(Guid user);


    // .. ����� ����� ����� ���������� ������ ��� ��������� ������ ������� ������, �������������� � ����� ������, ������������� ����������� �����, ��������� �������� � ��� ������������� � �.�.
    // .. �� ��� ��� �� ��������������.
    // .. �����! � � ��� ����!  :)
}
