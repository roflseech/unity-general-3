
namespace UnityGeneral
{
    public interface ITaggedObject<T>
    {
        public string Tag { get; }
        public T Value { get; }
    }
}


