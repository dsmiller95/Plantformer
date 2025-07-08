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
using Domain.Events;
using Domain.FunctionalMachine;
using Domain.StateInterfaces;
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
  private GodotCombat? _combat;
  private StateTicker _stateTicker;

  public PlayerController() {
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

    _stateTicker = new StateTicker(options);
  }

  public override void _Ready() {
    HurtboxPunch.AssertSetInInspector();
    HurtboxShape.AssertSetInInspector();

    _combat = new GodotCombat(HurtboxShape, HurtboxPunch);
  }

  public override void _PhysicsProcess(double delta) {
    HurtboxShape.AssertSetInInspector();

    var physics = GodotPhysics.CreateFromCharacterBody2D(this);
    var eventSink = new ListEventSink();
    var context = new CharacterContext(new GodotClock(delta), _input, physics, _combat ?? NullCharacterCombat.Instance, eventSink);

    HurtboxShape.Visible = false;

    _stateTicker.Tick(context);

    DebugDrawer?.AppendDraw(_stateTicker.CurrentDebugInfo(context));

    foreach (var outputEvent in eventSink.Events) {
      HandleEvent(this, outputEvent);
    }

    MoveAndSlide();
  }

  private static void HandleEvent(CharacterBody2D body, IOutputEvent outputEvent) {
    switch (outputEvent) {
      default:
        throw ExhaustiveMatch.Failed(outputEvent);
      case SetVerticalVelocity setVertical:
        body.Velocity = body.Velocity with { Y = -setVertical.Vertical };
        break;
      case SetHorizontalVelocity setHorizontal:
        body.Velocity = body.Velocity with { X = setHorizontal.Horizontal };
        if (setHorizontal.Facing.HasValue) {
          SetFacing(body, setHorizontal.Facing.Value);
        }
        break;
      case AddVerticalVelocity addVertical:
        body.Velocity += new Vector2(0, -addVertical.Vertical);
        break;
    }
  }

  private static void SetFacing(CharacterBody2D body, FacingDirection direction) {
    // negative scale actually becomes all fucky because these values are not stored directly
    //  they are derieved and applied directly to the transformation matrix
    //  and the matrix doesnt know the difference between -x VS -y and 180 deg rotation
    //  https://docs.godotengine.org/en/stable/classes/class_node2d.html#class-node2d-property-scale
    body.Rotation = 0f;
    body.Scale = new Vector2(
      direction switch {
        FacingDirection.Left => -1f,
        FacingDirection.Right => 1f,
        _ => throw ExhaustiveMatch.Failed(direction),
      },
      1);
  }

  private sealed class GodotClock(double deltaTime) : IClock {
    public float Now => Time.GetTicksMsec() / 1000f;
    public ulong Frame => Engine.GetPhysicsFrames();
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

  private sealed record GodotPhysics(bool IsGrounded, Vector2 Velocity) : ICharacterPhysics {
    public Vector2 Velocity { get; set; } = Velocity;

    public static GodotPhysics CreateFromCharacterBody2D(CharacterBody2D body) {
      return new GodotPhysics(body.IsOnFloor(), body.Velocity * new Vector2(1, -1));
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
