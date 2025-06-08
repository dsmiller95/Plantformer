namespace Plantformer.Domain.StateInterfaces;

using Chickensoft.Log;
using Character;

public class StateMachine(IState state) {
  private IState _state = state;
  private readonly Log _log = new(nameof(StateMachine), new ConsoleWriter());

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
