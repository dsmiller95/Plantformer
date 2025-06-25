namespace Plantformer.Domain.Character;

public interface ICharacterCombat {
  public bool Hit(HitType type);
}

public enum HitType {
  Punch,
}
