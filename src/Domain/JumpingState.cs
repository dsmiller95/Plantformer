namespace Plantformer.Domain;

using System;
using Chickensoft.Log;
using Godot;

public record JumpingState : IState {

  private float _jumpStartTime;

  public JumpingState(CharacterContext context, float Speed = 50f) {
    this.Speed = Speed;
    _jumpStartTime = context.Clock.Now;
  }

  public IState Tick(CharacterContext context) {

    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);
    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);

    return this;
  }

  public float Speed { get; init; }

  public void Deconstruct(out float Speed) {
    Speed = this.Speed;
  }
}
