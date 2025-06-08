namespace Plantformer.Domain.States;

using Character;

public record JumpingState(
  CharacterOptions Options,
  IStateDefinition WalkingState) : IState {

  private float _jumpStartTime;

  public IStateDefinition? Tick(CharacterContext context) {
    if(context.Clock.Now - _jumpStartTime > Options.JumpTime) {
      return WalkingState;
    }

    context.Physics.SetHorizontal(context.Input.MoveAxis * Options.MoveSpeed);
    context.Physics.SetVertical(Options.JumpSpeed);

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
