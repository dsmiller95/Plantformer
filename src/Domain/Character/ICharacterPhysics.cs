namespace Plantformer.Domain.Character;

using Godot;

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public float VerticalVelocity { get; }
  public void SetFacing(FacingDirection direction);
  public void SetHorizontal(float vx);
  public void SetVertical(float vy);

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
