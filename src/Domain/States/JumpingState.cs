namespace Plantformer.Domain.States;

using Character;

public record JumpingState(
  CharacterOptions Options,
  IStateDefinition WalkingState) : IState {

  private float _jumpStartTime;

  public IStateDefinition? Tick(CharacterContext context) {
    if(context.Physics.IsGrounded) {
      return WalkingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    if (context.Physics.VerticalVelocity > 0) {
      context.ApplyGravity(Options.JumpGravity);
    }else {
      context.ApplyGravity(Options.Gravity);
    }

    return null;
  }

  public void Enter(CharacterContext context) {
    _jumpStartTime = context.Clock.Now;
    context.Physics.SetVertical(Options.JumpSpeed);
  }

  public void Exit(CharacterContext context) {
    _jumpStartTime = -1;
    context.Physics.SetVertical(0);
  }
}
