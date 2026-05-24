using System;

namespace MemoryPack;

/// <summary>
/// Marks a class for pooled MemoryPack deserialization. When applied alongside
/// <see cref="MemoryPackableAttribute"/>, the Carbon-flavoured MemoryPack source generator emits
/// <c>Facepunch.Pool.Get&lt;T&gt;()</c> instead of <c>new T()</c> in the generated
/// <c>Deserialize</c> method's NEW branch.
/// </summary>
/// <remarks>
/// <para>Constraints (enforced as compile-time diagnostics by the generator):</para>
/// <list type="bullet">
/// <item><description>Only <c>class</c> types — value types and records with positional constructors are not supported.</description></item>
/// <item><description>Type must be reachable through <c>Facepunch.Pool.Get&lt;T&gt;()</c> (reference type with parameterless constructor or implementing <c>Pool.IPooled</c>).</description></item>
/// <item><description>Types with <c>[MemoryPackIgnore]</c> mutable fields should implement <c>Pool.IPooled.EnterPool()</c> to reset those fields on reuse — otherwise stale values leak across pool consumers.</description></item>
/// </list>
/// <para>The attribute is a marker only — the runtime ignores it. The Carbon fork's generator detects it by full name (<c>MemoryPack.PooledMemoryPackableAttribute</c>) during compilation.</para>
/// <para>This attribute lives in the Carbon-flavoured fork of MemoryPack.Core (not in upstream Cysharp/MemoryPack).</para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class PooledMemoryPackableAttribute : Attribute
{
}
