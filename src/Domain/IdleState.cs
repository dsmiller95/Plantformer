namespace Plantformer.Domain;

using System;
using Godot;



public record IdleState(
  IStateDefinition WalkingState,
  float DeadZone = 0.1f): IState {
  public IStateDefinition? Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > DeadZone) {
      return WalkingState;
    }

    return null;
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetHorizontal(0);
  }
}
