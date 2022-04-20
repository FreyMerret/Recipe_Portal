using RecipePortal.UserAccountService.Models;

namespace RecipePortal.UserAccountService;

public interface IUserAccountService
{
    Task<UserAccountModel> Create (RegisterUserAccountModel model);


    //SUBSCRIBE
    Task AddSubscriptionToAuthor(Guid subscriber, string autorNickname);

    Task AddSubscriptionToCategory(SubscriptionToCategoryModel model);

    Task AddSubscriptionToComments(SubscriptionToCommentsModel model);


    // .. ����� ����� ����� ���������� ������ ��� ��������� ������ ������� ������, �������������� � ����� ������, ������������� ����������� �����, ��������� �������� � ��� ������������� � �.�.
    // .. �� ��� ��� �� ��������������.
    // .. �����! � � ��� ����!  :)
}
