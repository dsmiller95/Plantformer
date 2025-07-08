namespace Plantformer.Domain.Character;

using Godot;

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public Vector2 Velocity { get; }
}

public enum FacingDirection {
  Left,
  Right,
}
