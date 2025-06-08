// PlayerController.cs  (attach to a CharacterBody2D)
/*
 * Engine-side glue for the functional state-machine you already have.
 * Requires:
 *   – The domain library containing  IClock, IInput, ICharacterPhysics,
 *     IState, StateMachine, CoyoteState, CharacterEngine, etc.
 */

using System;
using Godot;

namespace Plantformer;

using Chickensoft.Log;
using Domain;
using Domain.Character;
using Domain.States;

public partial class PlayerController : CharacterBody2D {
  // ---- Tunables ----------------------------------------------------------
  private const float WalkSpeed = 130f;
  private const float JumpSpeed = -480f; // negative = up in Godot
  private const float JumpTime = 1f; // negative = up in Godot
  private const float Gravity = 800f;
  private const float CoyoteSecs = 0.18f;
  // ------------------------------------------------------------------------

  // Thin wrappers that satisfy the domain interfaces
  private readonly GodotInput _input = new();
  private GodotPhysics _physics; // needs 'this' ⇒ create in _Ready
  private StateMachine _stateMachine;

  // ------------------------------------------------------------------------
  public override void _Ready() {
    _physics = new GodotPhysics(this);


    var entryState = BuildStateGraph();
    _stateMachine = new StateMachine(entryState);
  }

  private IState BuildStateGraph() {
    IState idle = null!;
    IState walking = null!;
    IState jumping = null!;

    idle = new IdleState(
          WalkingState: new LambdaStateDefinition(ctx => walking),
          JumpingState: new LambdaStateDefinition(ctx => jumping)
        );
    walking = new WalkingState(
          IdleState: new LambdaStateDefinition(ctx => idle),
          JumpingState: new LambdaStateDefinition(ctx => jumping),
          Speed: WalkSpeed
        );
    jumping = new JumpingState(
          WalkingState: new LambdaStateDefinition(ctx => walking),
          JumpTime: JumpTime,
          JumpSpeed: JumpSpeed
        );

    return idle;
  }

  public override void _PhysicsProcess(double delta) {
    var context = new CharacterContext(new GodotClock(delta), _input, _physics);
    _stateMachine.Tick(context);

    // Add gravity and push the body
    Velocity = Velocity with { Y = Velocity.Y + Gravity * (float)delta };
    MoveAndSlide();
  }

  // ────────────────────────────────────────────────────────────────────────
  //  Adapters
  // ────────────────────────────────────────────────────────────────────────
  private sealed class GodotClock(double deltaTime) : IClock {
    public float Now => Time.GetTicksMsec() / 1000f;
    public float DeltaTime => (float)deltaTime;
  }

  private sealed class GodotInput : IInput {
    public bool JumpPressed => Input.IsActionJustPressed("jump");
    public bool AttackPressed => Input.IsActionJustPressed("attack");
    public float MoveAxis => Input.GetAxis("move_left", "move_right");
    public bool CrouchPressed => Input.IsActionPressed("crouch");
  }

  private sealed class GodotPhysics(CharacterBody2D body) : ICharacterPhysics {
    public bool IsGrounded => body.IsOnFloor();
    public float VerticalVelocity => body.Velocity.Y;
    public void SetHorizontal(float vx) => body.Velocity = body.Velocity with { X = vx };
    public void SetVertical(float vy) => body.Velocity = body.Velocity with { Y = vy };
  }
}
