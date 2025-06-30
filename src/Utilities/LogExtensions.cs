using Chickensoft.Log;

namespace Plantformer.Utilities;

public static class LogExtensions {
  public static void Error(this Log log, string message) {
    log.Err(message);
  }

  public static void Info(this Log log, string message) {
    log.Print(message);
  }
}
