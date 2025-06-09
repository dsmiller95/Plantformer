namespace Plantformer.Domain.States;

using Character;
using StateInterfaces;

public class StateGraph {

  public static StateMachine Build(CharacterOptions options) {
    var idle = new SwappableStateDefinition();
    var walking = new SwappableStateDefinition();
    var jumping = new SwappableStateDefinition();
    var falling = new SwappableStateDefinition();

    idle.State = new IdleState(options,
      WalkingState: walking,
      JumpingState: jumping,
      UnGroundedState: falling
    );
    walking.State = new WalkingState(options,
      IdleState: idle,
      JumpingState: jumping,
      UnGroundedState: falling
    );
    jumping.State = new JumpingUpState(options,
      GroundedState: walking,
      FallingState: falling
    );
    falling.State = new FallingState(options,
      GroundedState: walking,
      JumpedState: jumping
    );

    return new StateMachine(idle.State);
  }
}
