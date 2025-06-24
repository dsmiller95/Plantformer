// PlayerController.cs  (attach to a CharacterBody2D)
/*
 * Engine-side glue for the functional state-machine you already have.
 * Requires:
 *   – The domain library containing  IClock, IInput, ICharacterPhysics,
 *     IState, StateMachine, CoyoteState, CharacterEngine, etc.
 */

using Godot;

namespace Plantformer;

using Domain.Character;
using Domain.FunctionalMachine;
using Domain.StateInterfaces;
using Domain.States;

public partial class PlayerController : CharacterBody2D {

  [Export]
  public PositionalDebugDrawer? DebugDrawer;

  private readonly GodotInput _input = new();
  private GodotPhysics _physics;
  private StateMachine _stateMachine;
  private StateTicker _stateTicker;

  public override void _Ready() {
    _physics = new GodotPhysics(this);

    var options = new CharacterOptions {
      MoveSpeed = 400f,
      JumpSpeed = 900f,
      Gravity = -3000f,
      DeadZone = 0.1f,
      JumpTime = 1f,
      JumpGravity = -2000,
      JumpArrestGravity = -5000,
      CoyoteTime = 0.18f,
      AirJumps = 1f, // double jump
    };

    _stateMachine = StateGraph.Build(options);
    _stateTicker = new StateTicker(options);
  }

  public override void _PhysicsProcess(double delta) {
    var context = new CharacterContext(new GodotClock(delta), _input, _physics);
    // _stateMachine.Tick(context);
    _stateTicker.Tick(context);
    DebugDrawer?.AppendDraw(_stateTicker.CurrentDebugInfo());

    MoveAndSlide();
  }

  private sealed class GodotClock(double deltaTime) : IClock {
    public float Now => Time.GetTicksMsec() / 1000f;
    public float DeltaTime => (float)deltaTime;
  }

  private sealed class GodotInput : IInput {
    public PressedState Jump => GetPressedState("jump");
    public PressedState Attack => GetPressedState("attack");
    public float MoveAxis => Input.GetAxis("move_left", "move_right");
    public PressedState Crouch => GetPressedState("crouch");

    private PressedState GetPressedState(string action) {
      if (Input.IsActionJustPressed(action)) {
        return new PressedState(false, true);
      }

      if (Input.IsActionJustReleased(action)) {
        return new PressedState(true, false);
      }

      if (Input.IsActionPressed(action)) {
        return new PressedState(true, true);
      }

      return new PressedState(false, false);
    }
  }

  private sealed class GodotPhysics(CharacterBody2D body) : ICharacterPhysics {
    public bool IsGrounded => body.IsOnFloor();
    public float VerticalVelocity => -body.Velocity.Y; // negative = up. flip so negative = down
    public void SetHorizontal(float vx) => body.Velocity = body.Velocity with { X = vx };
    public void SetVertical(float vy) => body.Velocity = body.Velocity with { Y = -vy };
  }
}
