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
using Domain.StateInterfaces;
using Domain.States;

public partial class PlayerController : CharacterBody2D {
  private const float WalkSpeed = 400f;
  private const float JumpSpeed = 900f;
  private const float JumpTime = 1f;
  private const float Gravity = -3000f;
  private const float JumpGravity = -2000f;
  private const float CoyoteSecs = 0.18f;

  private readonly GodotInput _input = new();
  private GodotPhysics _physics;
  private StateMachine _stateMachine;

  public override void _Ready() {
    _physics = new GodotPhysics(this);

    var options = new CharacterOptions {
      MoveSpeed = WalkSpeed,
      JumpSpeed = JumpSpeed,
      Gravity = Gravity,
      DeadZone = 0.1f,
      JumpTime = JumpTime,
      JumpGravity = JumpGravity,
      CoyoteTime = CoyoteSecs,
    };

    _stateMachine = StateGraph.Build(options);
  }

  public override void _PhysicsProcess(double delta) {
    var context = new CharacterContext(new GodotClock(delta), _input, _physics);
    _stateMachine.Tick(context);

    MoveAndSlide();
  }

  private sealed class GodotClock(double deltaTime) : IClock {
    public float Now => Time.GetTicksMsec() / 1000f;
    public float DeltaTime => (float)deltaTime;
  }

  private sealed class GodotInput : IInput {
    public bool JumpPressed => Input.IsActionPressed("jump");
    public bool AttackPressed => Input.IsActionPressed("attack");
    public float MoveAxis => Input.GetAxis("move_left", "move_right");
    public bool CrouchPressed => Input.IsActionPressed("crouch");
  }

  private sealed class GodotPhysics(CharacterBody2D body) : ICharacterPhysics {
    public bool IsGrounded => body.IsOnFloor();
    public float VerticalVelocity => -body.Velocity.Y; // negative = up. flip so negative = down
    public void SetHorizontal(float vx) => body.Velocity = body.Velocity with { X = vx };
    public void SetVertical(float vy) => body.Velocity = body.Velocity with { Y = -vy };
  }
}
