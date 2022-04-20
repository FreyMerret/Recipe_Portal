using System.Threading.Tasks;

namespace RecipePortal.RabbitMqService;

public delegate Task OnDataReceiveEvent<T>(T data);

public interface IRabbitMq
{
    Task Subscribe<T>(string queueName, OnDataReceiveEvent<T> onReceive);

    Task PushAsync<T>(string queueName, T data);
}
