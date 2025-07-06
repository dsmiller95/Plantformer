namespace Plantformer.Domain.Character;

using Godot;

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public Vector2 Velocity { get; set; }
  public FacingDirection Facing { get; set; }

  public float VerticalVelocity => Velocity.Y;

  public void SetFacing(FacingDirection direction) {
    Facing = direction;
  }
  public void SetHorizontal(float vx) {
    Velocity = Velocity with { X = vx };
  }
  public void SetVertical(float vy) {
    Velocity = Velocity with { Y = vy }; // negative = up. flip so negative = down
    GD.Print($"SetVertical: {vy}, Velocity: {Velocity}");
  }

  public void SetHorizontalAndFacing(float vx) {
    SetHorizontal(vx);
    if(Mathf.Abs(vx) > Mathf.Epsilon) {
      SetFacing(vx < 0 ? FacingDirection.Left : FacingDirection.Right);
    }
  }
}

public enum FacingDirection {
  Left,
  Right,
}
