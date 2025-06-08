namespace Plantformer.Domain.States;

using System;
using Character;

public record IdleState(
  CharacterOptions Options,
  IStateDefinition WalkingState,
  IStateDefinition JumpingState,
  IStateDefinition UnGroundedState): IState {
  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > Options.DeadZone) {
      return WalkingState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    if (!context.Physics.IsGrounded) {
      return UnGroundedState;
    }

    context.ApplyGravity(Options.Gravity);

    return null;
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetHorizontal(0);
  }
}
