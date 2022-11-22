
/// <summary>
/// Singleton pattern.
/// This class can be called from anywhere and only exists one instance at a time.
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
public class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        private set { }
        get
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }
    }

    // Private constructor prevents from instancing a sigleton with new.
    protected Singleton() { }
}