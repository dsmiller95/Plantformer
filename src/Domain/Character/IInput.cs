namespace Plantformer.Domain.Character;

public interface IInput {
  public bool JumpPressed { get; }
  public bool AttackPressed  { get; }
  public bool CrouchPressed { get; }
  public float MoveAxis { get; }
}
