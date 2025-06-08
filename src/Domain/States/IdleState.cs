namespace Plantformer.Domain.States;

using System;
using Character;

public record IdleState(
  IStateDefinition WalkingState,
  IStateDefinition JumpingState,
  float DeadZone = 0.1f,
  float G = 800f): IState {
  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > DeadZone) {
      return WalkingState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    context.ApplyGravity(G);

    return null;
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetHorizontal(0);
  }
}
