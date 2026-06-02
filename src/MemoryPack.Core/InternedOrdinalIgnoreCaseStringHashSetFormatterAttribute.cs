using System;
using System.Collections.Generic;
using MemoryPack.Formatters;

namespace MemoryPack;

public sealed class InternedOrdinalIgnoreCaseStringHashSetFormatterAttribute : MemoryPackCustomFormatterAttribute<InternedStringHashSetFormatter, HashSet<string?>>
{
	private static readonly InternedStringHashSetFormatter formatter = new InternedStringHashSetFormatter(StringComparer.OrdinalIgnoreCase);

	public override InternedStringHashSetFormatter GetFormatter()
	{
		return formatter;
	}
}
