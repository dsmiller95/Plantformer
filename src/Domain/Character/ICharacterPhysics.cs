namespace Plantformer.Domain.Character;

using Godot;

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public Vector2 Velocity { get; set; }
  public FacingDirection Facing { get; set; }

  public void SetHorizontalAndFacing(float vx) {
    Velocity = Velocity with { X = vx };
    if(Mathf.Abs(vx) > Mathf.Epsilon) {
      Facing = vx < 0 ? FacingDirection.Left : FacingDirection.Right;
    }
  }
}

public enum FacingDirection {
  Left,
  Right,
}
