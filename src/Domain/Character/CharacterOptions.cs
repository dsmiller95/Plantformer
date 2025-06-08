namespace Plantformer.Domain.Character;

public record CharacterOptions {
  public required float MoveSpeed { get; init; }
  public required float JumpSpeed { get; init; }
  public required float Gravity { get; init; }
  public required float DeadZone { get; init; }
  public required float JumpTime { get; init; }
}
