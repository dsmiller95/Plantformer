// PlayerController.cs  (attach to a CharacterBody2D)
/*
 * Engine-side glue for the functional state-machine you already have.
 * Requires:
 *   – The domain library containing  IClock, IInput, ICharacterPhysics,
 *     IState, StateMachine, CoyoteState, CharacterEngine, etc.
 */

using Godot;

namespace Plantformer;

using System.Linq;
using Chickensoft.Log;
using Domain.Character;
using Domain.FunctionalMachine;
using Domain.StateInterfaces;
using Domain.States;
using ExhaustiveMatching;
using Utilities;

public partial class PlayerController : CharacterBody2D {

  [Export]
  public PositionalDebugDrawer? DebugDrawer;
  [Export]
  public Area2D? HurtboxPunch;
  [Export]
  public CollisionShape2D? HurtboxShape;


  private readonly GodotInput _input = new();
  private readonly Log _log = new(nameof(PlayerController), new ConsoleWriter());
  private GodotPhysics _physics;
  private GodotCombat? _combat;
  private StateMachine _stateMachine;
  private StateTicker _stateTicker;

  public PlayerController() {
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
      AttackDuration = 0.2f,
    };

    _stateMachine = StateGraph.Build(options);
    _stateTicker = new StateTicker(options);
  }

  public override void _Ready() {
    HurtboxPunch.AssertSetInInspector();
    HurtboxShape.AssertSetInInspector();

    _combat = new GodotCombat(HurtboxShape, HurtboxPunch);
  }

  public override void _PhysicsProcess(double delta) {
    HurtboxShape.AssertSetInInspector();

    var context = new CharacterContext(new GodotClock(delta), _input, _physics, _combat ?? NullCharacterCombat.Instance);

    HurtboxShape.Visible = false;
    _stateTicker.Tick(context);
    DebugDrawer?.AppendDraw(_stateTicker.CurrentDebugInfo(context));

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

    private static PressedState GetPressedState(string action) {
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
    public void SetFacing(FacingDirection direction) {
      body.Scale = new Vector2(
        direction switch {
          FacingDirection.Left => -1f,
          FacingDirection.Right => 1f,
          _ => throw ExhaustiveMatch.Failed(direction),
        },
        body.Scale.Y);
    }

    public void SetHorizontal(float vx) {
      body.Velocity = body.Velocity with { X = vx };
    }

    public void SetVertical(float vy) {
      body.Velocity = body.Velocity with { Y = -vy };
    }
  }

  private sealed class GodotCombat(CollisionShape2D HurtboxShape, Area2D HurtboxArea) : ICharacterCombat {
    private readonly Log _log = new(nameof(PlayerController), new ConsoleWriter());
    public bool Hit(HitType type) {

      foreach (var hitTarget in HurtboxArea.GetOverlappingBodies()
                 .OfType<RigidBody2D>())
      {
        hitTarget.ApplyImpulse(new Vector2(1000, -1000f));
      }

      HurtboxShape.Visible = true;
      _log.Print($"Hit with {type}");
      return false;
    }
  }
}
