namespace Plantformer.Domain.States;

using System;
using System.Collections.Generic;
using Character;
using StateInterfaces;

public class StateGraph {

  private int _nextId = 1;
  private readonly Dictionary<StateId, IState> _states = new();

  private readonly record struct StateId {
    public required int Id { get; init; }
  }

  private StateId TakeId() {
    var id = new StateId { Id = _nextId };
    _nextId++;
    return id;
  }

  private class IdStateDefinition(StateId id, StateGraph graph) : IStateDefinition {
    public IState CreateState(CharacterContext context) => graph._states[id];
  }

  private IStateDefinition To(StateId id) => new IdStateDefinition(id, this);
  private T Define<T>(StateId id, T state) where T: IState {
    if (!_states.TryAdd(id, state)) {
      throw new InvalidOperationException($"State with id {id.Id} already exists in the state graph.");
    }

    return state;
  }

  private IState Get(StateId id) => _states[id];

  private StateMachine BuildSelf(CharacterOptions options) {
    var idleId = TakeId();
    var walkingId = TakeId();
    var jumpingId = TakeId();
    var fallingId = TakeId();

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
      GroundedState: To(walkingId),
      FallingState: To(fallingId)
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
