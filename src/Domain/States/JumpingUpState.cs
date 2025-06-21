namespace Plantformer.Domain.States;

using System;
using Character;
using StateInterfaces;

public record JumpingUpState(
  CharacterOptions Options,
  IStateDefinition FallingState) : IState {
  public IStateDefinition? Transition(CharacterContext context) {
    if (!context.Input.JumpPressed || context.Physics.VerticalVelocity < 0) {
      return FallingState;
    }

    return null;
  }

  public void Tick(CharacterContext context) {
    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.ApplyGravity(Options.JumpGravity);
  }

  public void Enter(CharacterContext context) {
    context.Physics.SetVertical(Options.JumpSpeed);
  }

  public void Exit(CharacterContext context) {
    context.Physics.SetVertical(Math.Min(0, context.Physics.VerticalVelocity));
  }
}
