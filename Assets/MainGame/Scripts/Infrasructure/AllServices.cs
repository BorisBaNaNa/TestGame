public class AllServices
{
    private class Implementation <TService> where TService : IService
    {
        public static TService Instance;
    }

    // Готовить ногу и бинты!
    // public static AllServices Instance => _instance ??= new();

    // private static AllServices _instance ;

    public static void RegisterService<TService>(TService instanceObj) where TService : IService
        => Implementation<TService>.Instance = instanceObj;

    public static TService GetService<TService>() where TService : IService
        => Implementation<TService>.Instance;
}

public interface IService { }