namespace Plantformer.Domain.States;

using System;
using Character;
using StateMachine;

public record WalkingState(
  CharacterOptions Options,
  IStateDefinition IdleState,
  IStateDefinition JumpingState,
  IStateDefinition UnGroundedState): IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < Options.DeadZone) {
      return IdleState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    if (!context.Physics.IsGrounded) {
      return UnGroundedState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.Gravity);

    return null;
  }
}
