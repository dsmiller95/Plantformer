namespace Plantformer.Domain.States;

using System;
using Character;
using StateInterfaces;

public record IdleState(
  CharacterOptions Options,
  IStateDefinition WalkingState,
  IStateDefinition JumpingState,
  IStateDefinition UnGroundedState): IState {

  public IStateDefinition? Transition(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > Options.DeadZone) {
      return WalkingState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    if (!context.Physics.IsGrounded) {
      return UnGroundedState;
    }

    return null;
  }

  public void Tick(CharacterContext context) {
    context.ApplyGravity(Options.Gravity);
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetHorizontal(0);
  }
}
