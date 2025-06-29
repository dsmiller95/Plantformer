namespace Plantformer.Utilities;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Chickensoft.Log;

public static class GodotNullExtensions {
  private static readonly Log _log = new(nameof(PlayerController), new ConsoleWriter());
  public static void AssertSetInInspector<T>(
    [NotNull] this T? value,
    [CallerArgumentExpression("value")] string valueName = "",
    [CallerFilePath] string filePath = "") where T : class {
    if (value == null) {
      var msg = $"Property {valueName} must be set in the inspector. Error in {filePath}";
      _log.Error(msg);
      throw new InvalidOperationException(msg);
    }
  }
}
