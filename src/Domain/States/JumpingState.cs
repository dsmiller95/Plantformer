namespace Plantformer.Domain.States;

using Character;

public record JumpingState(
  IStateDefinition WalkingState,
  float JumpTime = 1,
  float Speed = 50f,
  float JumpSpeed = 50f) : IState {

  private float _jumpStartTime;

  public IStateDefinition? Tick(CharacterContext context) {
    if(context.Clock.Now - _jumpStartTime > JumpTime) {
      return WalkingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Speed);
    context.Physics.SetVertical(JumpSpeed);

    return null;
  }

  public void Enter(CharacterContext context) {
    _jumpStartTime = context.Clock.Now;
  }

  public void Exit(CharacterContext context) {
    _jumpStartTime = -1;
    context.Physics.SetVertical(0);
  }
}
