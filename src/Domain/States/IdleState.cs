namespace Plantformer.Domain.States;

using System;
using Character;

public record IdleState(
  CharacterOptions Options,
  IStateDefinition WalkingState,
  IStateDefinition JumpingState): IState {
  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > Options.DeadZone) {
      return WalkingState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    context.ApplyGravity(Options.Gravity);

    return null;
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetHorizontal(0);
  }
}
