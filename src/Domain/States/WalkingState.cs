namespace Plantformer.Domain.States;

using System;
using Character;

public record WalkingState(
  IStateDefinition IdleState,
  IStateDefinition JumpingState,
  float DeadZone = 0.1f,
  float Speed = 50f,
  float G = 800f): IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < DeadZone) {
      return IdleState;
    }

    if(context.Input.JumpPressed) {
      return JumpingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);
    context.ApplyGravity(G);

    return null;
  }
}
