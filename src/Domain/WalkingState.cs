namespace Plantformer.Domain;

using System;
using Godot;

public record WalkingState(
  IStateDefinition IdleState,
  float DeadZone = 0.1f,
  float Speed = 50f): IState {

  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < DeadZone) {
      return IdleState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);

    return null;
  }
}
