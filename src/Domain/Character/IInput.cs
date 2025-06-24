namespace Plantformer.Domain.Character;

public interface IInput {
  public PressedState Jump { get; }
  public PressedState Attack  { get; }
  public PressedState Crouch { get; }
  public float MoveAxis { get; }
}

public readonly record struct PressedState(bool DownLastFrame, bool Down)
{
  public bool Pressed => !DownLastFrame && Down;
  public bool Released => DownLastFrame && !Down;
  public bool Held => DownLastFrame && Down;
}
