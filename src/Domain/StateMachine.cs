namespace Plantformer.Domain;

using System;
using Chickensoft.Log;

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

public interface IStateDefinition {
  public IState CreateState(CharacterContext context);
}

public class LambdaStateDefinition(Func<CharacterContext, IState> creator) : IStateDefinition {
  public IState CreateState(CharacterContext context) => creator(context);

  // implicit conversion from labmda
  public static implicit operator LambdaStateDefinition(Func<CharacterContext, IState> creator) => new(creator);
}

public interface IState {
  public void Enter(CharacterContext context) { }
  public IStateDefinition? Tick(CharacterContext context);
  public void Exit(CharacterContext context) { }
}

public class StateMachine(IState state) {
  private IState _state = state;
  private readonly Log _log = new(nameof(StateMachine), new ConsoleWriter(), new TraceWriter());

  public void Tick(CharacterContext context) {
    var transition = _state.Tick(context);
    if (transition == null) {
      return;
    }
    var nextState = transition.CreateState(context);

    _log.Print($"Transitioning from {_state.GetType().Name} to {nextState.GetType().Name}");

    _state.Exit(context);
    nextState.Enter(context);
    _state = nextState;
  }
}
