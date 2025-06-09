namespace Plantformer.Domain.StateInterfaces;

using System;
using Character;


public interface IState {
  public void Enter(CharacterContext context) { }
  public IStateDefinition? Transition(CharacterContext context);
  public void Tick(CharacterContext context);
  public void Exit(CharacterContext context) { }
}

public class TransitioningState(IStateDefinition to, Func<CharacterContext, bool> when, IState wrapped): IState {
  public IStateDefinition? Transition(CharacterContext context) {
    if (when(context)) {
      return to;
    }

    return wrapped.Transition(context);
  }

  public void Tick(CharacterContext context) => wrapped.Tick(context);
  public void Enter(CharacterContext context) => wrapped.Enter(context);
  public void Exit(CharacterContext context) => wrapped.Exit(context);
}

public static class IStateExtensions {
  public static IState TransitionsTo(this IState wrapped, IStateDefinition otherState, Func<CharacterContext, bool> when) =>
    new TransitioningState(otherState, when, wrapped);
}
