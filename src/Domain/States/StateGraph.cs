namespace Plantformer.Domain.States;

using Character;
using StateInterfaces;

public class StateGraph {

  public static StateMachine Build(CharacterOptions options) {
    IState idle = null!;
    IState walking = null!;
    IState jumping = null!;
    IState falling = null!;

    idle = new IdleState(options,
      WalkingState: new LambdaStateDefinition(ctx => walking),
      JumpingState: new LambdaStateDefinition(ctx => jumping),
      UnGroundedState: new LambdaStateDefinition(ctx => falling)
    );
    walking = new WalkingState(options,
      IdleState: new LambdaStateDefinition(ctx => idle),
      JumpingState: new LambdaStateDefinition(ctx => jumping),
      UnGroundedState: new LambdaStateDefinition(ctx => falling)
    );
    jumping = new JumpingUpState(options,
      GroundedState: new LambdaStateDefinition(ctx => walking),
      FallingState: new LambdaStateDefinition(ctx => falling)
    );
    falling = new FallingState(options,
      GroundedState: new LambdaStateDefinition(ctx => walking),
      JumpedState: new LambdaStateDefinition(ctx => jumping)
    );

    return new StateMachine(idle);
  }
}
