namespace Plantformer.Domain;

using System;
using Godot;



public record IdleState(float DeadZone = 0.1f): IState {
  public IState Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) > DeadZone) {
      GD.Print("started moving");
      return new WalkingState();
    }

    return this;
  }
}
