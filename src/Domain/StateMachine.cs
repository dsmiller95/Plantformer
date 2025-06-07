namespace Plantformer.Domain;


public interface IClock {
  public float Now { get; }
}

public interface IInput {
  public bool JumpPressed { get; }
  public bool AttackPressed  { get; }
  public bool CrouchPressed { get; }
  public float MoveAxis { get; }
}

public interface ICharacterPhysics {
  public bool IsGrounded { get; }
  public float VerticalVelocity { get; }

  public void SetHorizontal(float vx);

  public void SetVertical(float vy);
}

public record CharacterContext(IClock Clock, IInput Input, ICharacterPhysics Physics);

public interface IState {
  public IState Tick(CharacterContext context);
}

public record StateMachine(IState State) {

  public StateMachine Tick(CharacterContext context) {
    var nextState = State.Tick(context);
    if(nextState == State) {
      return this;
    }

    return new StateMachine(nextState);
  }


}
