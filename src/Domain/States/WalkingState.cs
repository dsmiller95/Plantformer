namespace Plantformer.Domain.States;

using System;
using Character;

public record WalkingState(
  CharacterOptions Options,
  IStateDefinition IdleState,
  IStateDefinition JumpingState): IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < Options.DeadZone) {
      return IdleState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.Gravity);

    return null;
  }
}
