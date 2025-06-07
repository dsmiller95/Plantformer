namespace Plantformer.Domain;

using System;
using Chickensoft.Log;
using Godot;

public record WalkingState(float DeadZone = 0.1f, float Speed = 50f) : IState {
  public IState Tick(CharacterContext context) {
    if(Math.Abs(context.Input.MoveAxis) < DeadZone) {

      GD.Print("stopped moving");
      return new IdleState();
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);

    return this;
  }
}
