namespace Plantformer.Domain.Character;

using Chickensoft.Log;

public interface ICharacterCombat {
  public bool Hit(HitType type);
}

public class NullCharacterCombat : ICharacterCombat {
  public static ICharacterCombat Instance { get; } = new NullCharacterCombat();

  private NullCharacterCombat(){}

  private readonly Log _log = new(nameof(NullCharacterCombat), new ConsoleWriter());
  public bool Hit(HitType type) {
    _log.Warn($"Hit {type}");
    return false;
  }
}

public enum HitType {
  Punch,
}
