namespace Plantformer.Domain.Character;

public record CharacterOptions {
  public required float MoveSpeed { get; init; }
  public required float Gravity { get; init; }
  public required float DeadZone { get; init; }
  public required float JumpTime { get; init; }
  public required float JumpSpeed { get; init; }
  public required float JumpGravity { get; init; }
  public required float JumpArrestGravity { get; init; }
  public required float CoyoteTime { get; init; }
  /// <summary>
  /// Number of jumps that can be made after the first jump off the ground (or coyotied)
  /// </summary>
  public required float AirJumps { get; init; }

  public required float AttackDuration { get; init; }
}
