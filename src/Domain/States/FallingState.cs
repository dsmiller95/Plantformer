namespace Plantformer.Domain.States;

using Character;
using StateInterfaces;

public record FallingState(
  CharacterOptions Options,
  IStateDefinition GroundedState,
  IStateDefinition JumpedState) : IState {

  private float _startedFallingTime;

  public IStateDefinition? Transition(CharacterContext context) {
    if(context.Physics.IsGrounded) {
      return GroundedState;
    }

    if(context.Input.JumpPressed && context.Clock.TimeSince(_startedFallingTime) < Options.CoyoteTime) {
      return JumpedState;
    }

    return null;
  }

  public void Tick(CharacterContext context) {
    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.Gravity);
  }

  public void Enter(CharacterContext context) {
    _startedFallingTime = context.Clock.Now;
  }

  public void Exit(CharacterContext context) {
    _startedFallingTime = -1;
  }
}
