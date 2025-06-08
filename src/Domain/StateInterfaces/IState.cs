namespace Plantformer.Domain.StateInterfaces;

using System;
using Character;

public interface IState {
  public void Enter(CharacterContext context) { }
  public IStateDefinition? Tick(CharacterContext context);
  public void Exit(CharacterContext context) { }
}

public class TransitioningState(IStateDefinition to, Func<CharacterContext, bool> when, IState wrapped): IState {
  public IStateDefinition? Tick(CharacterContext context) {
    if (when(context)) {
      return to;
    }

    return wrapped.Tick(context);
  }
}

public static class IStateExtensions {
  public static IState TransitionsTo(this IState wrapped, IStateDefinition otherState, Func<CharacterContext, bool> when) =>
    new TransitioningState(otherState, when, wrapped);
}
