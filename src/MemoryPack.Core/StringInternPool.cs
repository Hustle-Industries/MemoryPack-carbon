using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MemoryPack;

public static class StringInternPool
{
	public const int DefaultCapacity = 16384;

	static readonly ConcurrentDictionary<string, string> _pool = new(StringComparer.Ordinal);
	static int _capacity = DefaultCapacity;
	static int _count;

	public static int Count => Volatile.Read(ref _count);
	public static int Capacity => Volatile.Read(ref _capacity);

	public static void Configure(int capacity)
	{
		if (capacity < 0) capacity = 0;
		Volatile.Write(ref _capacity, capacity);
	}

	public static string? Intern(string? value)
	{
		if (value == null) return null;
		if (value.Length == 0) return string.Empty;

		if (_pool.TryGetValue(value, out var canonical)) return canonical;

		if (Volatile.Read(ref _count) >= Volatile.Read(ref _capacity)) return value;

		var added = _pool.GetOrAdd(value, value);
		if (ReferenceEquals(added, value)) Interlocked.Increment(ref _count);
		return added;
	}

	public static void Clear()
	{
		_pool.Clear();
		Volatile.Write(ref _count, 0);
	}
}
