namespace QueryFramework.Abstractions
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
