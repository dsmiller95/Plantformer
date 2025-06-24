namespace Plantformer.Domain.States;

using System;
using Character;
using StateInterfaces;

public record WalkingState(
  CharacterOptions Options,
  IStateDefinition IdleState,
  IStateDefinition JumpingState,
  IStateDefinition UnGroundedState): IState {

  public IStateDefinition? Transition(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < Options.DeadZone) {
      return IdleState;
    }

    if(context.Input.Jump.Down) {
      return JumpingState;
    }

    if (!context.Physics.IsGrounded) {
      return UnGroundedState;
    }

    return null;
  }

  public void Tick(CharacterContext context) {
    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.Gravity);
  }
}
