namespace Plantformer.Domain;

using System;
using Chickensoft.Log;
using Godot;

public record JumpingState(float Speed = 50f) : IState {

  private Mut? _m;

  private record Mut(float JumpStartTime) {

  }

  public IStateDefinition? Tick(CharacterContext context) {
    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);

    return null;
  }

  public void Enter(CharacterContext context) {
    _m = new Mut(context.Clock.Now);
  }

  public void Exit(CharacterContext context) {
    _m = null;
  }
}
