#pragma warning disable CS8618
#pragma warning disable CS0649

using System.Runtime.CompilerServices;

namespace MemoryPack.Internal;

internal static class CollectionsMarshalEx
{
    public static Span<T?> AsSpan<T>(List<T?> list)
    {
        ref var view = ref Unsafe.As<List<T?>, ListView<T?>>(ref list);
        return view._items.AsSpan(0, view._size);
    }

    public static Span<T?> CreateSpan<T>(List<T?> list, int length)
    {
        if (list.Capacity < length)
        {
            list.Capacity = length;
        }

        ref var view = ref Unsafe.As<List<T?>, ListView<T?>>(ref list);
        view._size = length;
        return view._items.AsSpan(0, length);
    }

    public static Span<T?> AsSpan<T>(Stack<T?> stack)
    {
        ref var view = ref Unsafe.As<Stack<T?>, StackView<T?>>(ref stack);
        return view._items.AsSpan(0, view._size);
    }

    public static Span<T?> CreateSpan<T>(Stack<T?> stack, int length)
    {
        ref var view = ref Unsafe.As<Stack<T?>, StackView<T?>>(ref stack);
        if (view._items.Length < length)
        {
            Array.Resize(ref view._items, length);
        }
        view._size = length;
        return view._items.AsSpan(0, length);
    }

    internal sealed class ListView<T>
    {
        public T[] _items;
        public int _size;
        public int _version;
    }

    internal sealed class StackView<T>
    {
        public T[] _items;
        public int _size;
        public int _version;
    }
}
