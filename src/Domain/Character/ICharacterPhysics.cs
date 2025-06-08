namespace Plantformer.Domain.Character;

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public float VerticalVelocity { get; }
  public void SetHorizontal(float vx);
  public void SetVertical(float vy);
}
