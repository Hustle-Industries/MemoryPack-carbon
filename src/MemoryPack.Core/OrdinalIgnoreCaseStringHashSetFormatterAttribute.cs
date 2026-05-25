using System;
using System.Collections.Generic;
using MemoryPack.Formatters;

namespace MemoryPack;

public sealed class OrdinalIgnoreCaseStringHashSetFormatterAttribute : MemoryPackCustomFormatterAttribute<HashSetFormatter<string>, HashSet<string?>>
{
	private static readonly HashSetFormatter<string> formatter = new HashSetFormatter<string>(StringComparer.OrdinalIgnoreCase);

	public override HashSetFormatter<string> GetFormatter()
	{
		return formatter;
	}
}
