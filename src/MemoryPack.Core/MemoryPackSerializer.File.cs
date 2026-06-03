using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryPack;

public static partial class MemoryPackSerializer
{
	public static async ValueTask SaveToFileAsync<T>(
		string path,
		T? value,
		MemoryPackSerializerOptions? options = null,
		bool atomic = true,
		CancellationToken cancellationToken = default)
	{
		var directory = Path.GetDirectoryName(path);
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
			Directory.CreateDirectory(directory);

		var target = atomic ? path + ".tmp" : path;

		if (atomic)
		{
			try { if (File.Exists(target)) File.Delete(target); }
			catch { }
		}

		using (var fs = new FileStream(
			target,
			FileMode.Create,
			FileAccess.Write,
			FileShare.None,
			bufferSize: 64 * 1024,
			FileOptions.Asynchronous))
		{
			await SerializeAsync(fs, value, options, cancellationToken).ConfigureAwait(false);
		}

		if (!atomic) return;

		if (File.Exists(path))
			File.Replace(target, path, destinationBackupFileName: null, ignoreMetadataErrors: true);
		else
			File.Move(target, path);
	}

	public static async ValueTask<T?> LoadFromFileAsync<
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
#endif
		T>(
		string path,
		MemoryPackSerializerOptions? options = null,
		CancellationToken cancellationToken = default)
	{
		using var fs = new FileStream(
			path,
			FileMode.Open,
			FileAccess.Read,
			FileShare.Read,
			bufferSize: 64 * 1024,
			FileOptions.Asynchronous | FileOptions.SequentialScan);
		return await DeserializeAsync<T>(fs, options, cancellationToken).ConfigureAwait(false);
	}
}
