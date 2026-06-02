using System.Collections.Generic;
using MemoryPack.Internal;

namespace MemoryPack.Formatters;

[Preserve]
public sealed class InternedStringHashSetFormatter : MemoryPackFormatter<HashSet<string?>>
{
	readonly IEqualityComparer<string?>? equalityComparer;

	public InternedStringHashSetFormatter()
		: this(null)
	{
	}

	public InternedStringHashSetFormatter(IEqualityComparer<string?>? equalityComparer)
	{
		this.equalityComparer = equalityComparer;
	}

	[Preserve]
	public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref HashSet<string?>? value)
	{
		if (value == null)
		{
			writer.WriteNullCollectionHeader();
			return;
		}

		writer.WriteCollectionHeader(value.Count);
		foreach (var item in value)
		{
			writer.WriteString(item);
		}
	}

	[Preserve]
	public override void Deserialize(ref MemoryPackReader reader, scoped ref HashSet<string?>? value)
	{
		if (!reader.TryReadCollectionHeader(out var length))
		{
			value = null;
			return;
		}

		if (value == null)
		{
			value = new HashSet<string?>(length, equalityComparer);
		}
		else
		{
			value.Clear();
		}

		for (int i = 0; i < length; i++)
		{
			var s = reader.ReadString();
			value.Add(StringInternPool.Intern(s));
		}
	}
}
