namespace Plantformer.Domain.Events;

using Character;
using ExhaustiveMatching;
using Godot;

public record SetHorizontalVelocity : IOutputEvent {
  public SetHorizontalVelocity(float Horizontal, bool SetFacing) {
    this.Horizontal = Horizontal;

    if(SetFacing && Mathf.Abs(Horizontal) > Mathf.Epsilon) {
      Facing = Horizontal < 0 ? FacingDirection.Left : FacingDirection.Right;
    }
  }

  public float Horizontal { get; init; }
  public FacingDirection? Facing { get; init; }
}

[Closed(typeof(SetHorizontalVelocity))]
public partial interface IOutputEvent;
