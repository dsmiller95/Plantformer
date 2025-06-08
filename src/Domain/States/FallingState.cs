namespace Plantformer.Domain.States;

using Character;
using StateMachine;

public record FallingState(
  CharacterOptions Options,
  IStateDefinition GroundedState) : IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(context.Physics.IsGrounded) {
      return GroundedState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.Gravity);

    return null;
  }
}
