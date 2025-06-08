namespace Plantformer.Domain.States;

using Character;
using StateMachine;

public record JumpingUpState(
  CharacterOptions Options,
  IStateDefinition GroundedState,
  IStateDefinition FallingState) : IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(context.Physics.IsGrounded) {
      return GroundedState;
    }

    if (!context.Input.JumpPressed || context.Physics.VerticalVelocity < 0) {
      return FallingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.JumpGravity);

    return null;
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetVertical(Options.JumpSpeed);
  }

  public void Exit(CharacterContext context) {
  }
}
