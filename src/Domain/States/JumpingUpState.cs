namespace Plantformer.Domain.States;

using System;
using Character;
using StateInterfaces;

public record JumpingUpState(
  CharacterOptions Options,
  IStateDefinition FallingState) : IState {
  public IStateDefinition? Transition(CharacterContext context) {
    if (!context.Input.Jump.Down || context.Physics.Velocity.Y < 0) {
      return FallingState;
    }

    return null;
  }

  public void Tick(CharacterContext context) {
    var vx = context.Input.MoveAxis * Options.MoveSpeed;
    context.Physics.Velocity = context.Physics.Velocity with { X = vx };
    context.ApplyGravity(Options.JumpGravity);
  }

  public void Enter(CharacterContext context) {
    context.Physics.Velocity = context.Physics.Velocity with { Y = Options.JumpSpeed };
  }

  public void Exit(CharacterContext context) {
    var vy = Math.Min(0, context.Physics.Velocity.Y);
    context.Physics.Velocity = context.Physics.Velocity with { Y = vy };
  }
}
