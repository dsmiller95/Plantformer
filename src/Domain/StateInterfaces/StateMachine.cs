namespace Plantformer.Domain.StateInterfaces;

using System;
using Chickensoft.Log;
using Character;

public class StateMachine(IState state) {
  private IState _state = state;
  private readonly Log _log = new(nameof(StateMachine), new ConsoleWriter());

  public void Tick(CharacterContext ctx) {
    var firstState = _state;
    TransitionTillSettled(ctx);
    if(firstState != _state) {
      _log.Print($"Transitioned from {firstState.GetType().Name} to {_state.GetType().Name}");
    }

    _state.Tick(ctx);
  }

  private void TransitionTillSettled(CharacterContext ctx) {
    while (true) {
      var nextState = _state.Transition(ctx)?.CreateState(ctx);
      if (nextState == null) {
        return;
      }

      _state.Exit(ctx);
      nextState.Enter(ctx);
      _state = nextState;
    }
  }
}
