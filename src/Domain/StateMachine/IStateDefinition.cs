namespace Plantformer.Domain.StateMachine;

using System;
using Character;

public interface IStateDefinition {
  public IState CreateState(CharacterContext context);
}


public class LambdaStateDefinition(Func<CharacterContext, IState> creator) : IStateDefinition {
  public IState CreateState(CharacterContext context) => creator(context);

  public static implicit operator LambdaStateDefinition(Func<CharacterContext, IState> creator) => new(creator);
}

public static class StateDefinitionExtensions {
  public static IStateDefinition ToFactory(this IState state) => new LambdaStateDefinition(_ => state);
}
