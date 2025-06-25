namespace Plantformer.Domain.States;

using System;
using System.Collections.Generic;
using Character;
using StateInterfaces;

public class StateGraph {

  private int _nextId = 1;
  private readonly Dictionary<StateId, IState> _states = new();

  private record StateId {
    public required int Id { get; init; }
  }
  private sealed record StateId<T> : StateId where T: IState { };

  private StateId<T> TakeId<T>() where T: IState {
    var id = new StateId<T> { Id = _nextId };
    _nextId++;
    return id;
  }

  private class IdStateDefinition(StateId id, StateGraph graph) : IStateDefinition {
    public IState CreateState(CharacterContext context) => graph._states[id];
  }

  private IStateDefinition To(StateId id) => new IdStateDefinition(id, this);
  private IStateDefinition To<T>(StateId<T> id) where T: IState {
    return new IdStateDefinition(id, this);
  }
  private IStateDefinition To<TFrom, TTo>(StateId<TFrom> id, Func<TFrom, TTo> transformState)
    where TFrom: class, IState
    where TTo: IState {
    return new LambdaStateDefinition(ctx => transformState(Get(id)));
  }

  private T Define<T>(StateId<T> id, T state) where T: IState {
    if (!_states.TryAdd(id, state)) {
      throw new InvalidOperationException($"State with id {id.Id} already exists in the state graph.");
    }

    return state;
  }

  private IState Get(StateId id) => _states[id];
  private T Get<T>(StateId<T> id) where T: class, IState => _states[id] as T ?? throw new InvalidOperationException($"State with id {id.Id} is not of type {typeof(T).Name}.");

  private StateMachine BuildSelf(CharacterOptions options) {
    var idleId = TakeId<IdleState>();
    var walkingId = TakeId<WalkingState>();
    var jumpingId = TakeId<JumpingUpState>();
    var fallingId = TakeId<FallingState>();

    var idle = Define(idleId, new IdleState(options,
      WalkingState: To(walkingId),
      JumpingState: To(jumpingId),
      UnGroundedState: To(fallingId)
    ));

    var walking = Define(walkingId, new WalkingState(options,
      IdleState: To(idleId),
      JumpingState: To(jumpingId),
      UnGroundedState: To(fallingId)
    ));

    var jumping = Define(jumpingId, new JumpingUpState(options,
      FallingState: To(fallingId, x => x.CantJump())
    ));

    var falling = Define(fallingId, new FallingState(options,
      GroundedState: To(walkingId),
      JumpedState: To(jumpingId)
    ));

    return new StateMachine(Get(idleId));
  }

  public static StateMachine Build(CharacterOptions options) {
    return new StateGraph().BuildSelf(options);
  }
}
