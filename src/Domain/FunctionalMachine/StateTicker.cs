namespace Plantformer.Domain.FunctionalMachine;

using System;
using Character;
using Chickensoft.Log;
using Godot;

public class StateTicker(CharacterOptions options) {

  private State _state = State.Idle;
  private float _lastGroundedTime = -1;
  private readonly Log _log = new(nameof(StateTicker), new ConsoleWriter());
  private int _jumpsSinceGrounded = 0;


  public DebugInfo CurrentDebugInfo(CharacterContext ctx) => _state switch {
    State.Idle => new DebugInfo(Colors.Black, "Idle"),
    State.Walking => new DebugInfo(Colors.Yellow, "Walking"),
    State.JumpingUp => new DebugInfo(Colors.Blue, "Jumping Up"),
    State.Falling when !IsCoyoteFalling(ctx) => new DebugInfo(Colors.Red, "Falling"),
    State.Falling when IsCoyoteFalling(ctx) => new DebugInfo(Colors.Purple, "Coyote"),
    _ => throw new ArgumentOutOfRangeException(nameof(_state), _state, null)
  };

  public void Tick(CharacterContext ctx) {
    var firstState = _state;
    TryTransitionTillSettled(ctx);
    if(firstState != _state) {
      _log.Print($"Transitioned from {firstState} to {_state}");
    }
    TickState(_state, ctx);
  }

  private void TryTransitionTillSettled(CharacterContext ctx) {
    var limit = 10;
    while (limit-- > 0) {
      var nextState = CheckTransition(_state, ctx);
      if (nextState == null) {
        return;
      }

      Exit(_state, ctx);
      _state = nextState.Value;
      Enter(_state, ctx);
    }
    _log.Err($"State transition did not settle after 10 iterations. Current state: {_state}, Context: {ctx}");
  }

  private void TickState(State state, CharacterContext ctx) {
    switch (state) {
      case State.Idle:
        ctx.ApplyGravity(options.Gravity);
        break;
      case State.Walking:
        ctx.Physics.SetHorizontal(ctx.Input.MoveAxis * options.MoveSpeed);
        ctx.ApplyGravity(options.Gravity);
        break;
      case State.JumpingUp:
        ctx.Physics.SetHorizontal(ctx.Input.MoveAxis * options.MoveSpeed);
        if (ctx.Input.Jump.Down) {
          ctx.ApplyGravity(options.JumpGravity);
        }
        else {
          ctx.ApplyGravity(options.JumpArrestGravity);
        }
        break;
      case State.Falling:
        ctx.Physics.SetHorizontal(ctx.Input.MoveAxis * options.MoveSpeed);
        ctx.ApplyGravity(options.Gravity);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(state), state, null);
    }
  }

  private State? CheckTransition(State current, CharacterContext ctx) {
    switch(current) {
      case State.Idle:
        if(Math.Abs(ctx.Input.MoveAxis) > options.DeadZone) {
          return State.Walking;
        }
        if(ctx.Input.Jump.Pressed) {
          return State.JumpingUp;
        }
        if (!ctx.Physics.IsGrounded) {
          return State.Falling;
        }
        break;

      case State.Walking:
        if(Math.Abs(ctx.Input.MoveAxis) < options.DeadZone) {
          return State.Idle;
        }
        if(ctx.Input.Jump.Pressed) {
          return State.JumpingUp;
        }
        if (!ctx.Physics.IsGrounded) {
          return State.Falling;
        }
        break;

      case State.JumpingUp:
        if (ctx.Physics.VerticalVelocity < 0) {
          return State.Falling;
        }

        break;

      case State.Falling:
        if (ctx.Physics.IsGrounded) {
          return State.Idle;
        }

        if (ctx.Input.Jump.Pressed) {
          if (IsCoyoteFalling(ctx)) {
            return State.JumpingUp;
          }
          if(_jumpsSinceGrounded <= options.AirJumps) {
            return State.JumpingUp;
          }
        }
        break;
    }

    return null;
  }

  private void Enter(State state, CharacterContext ctx) {
    switch(state) {
      case State.Idle:
        ctx.Physics.SetHorizontal(0);
        break;

      case State.Walking:
        break;

      case State.JumpingUp:
        ctx.Physics.SetVertical(options.JumpSpeed);
        _jumpsSinceGrounded += 1;
        break;

      case State.Falling:
        break;
    }
  }

  private void Exit(State state, CharacterContext ctx) {
    switch(state) {
      case State.Idle:
        _lastGroundedTime = ctx.Clock.Now;
        _jumpsSinceGrounded = 0;
        break;

      case State.Walking:
        _lastGroundedTime = ctx.Clock.Now;
        _jumpsSinceGrounded = 0;
        break;

      case State.JumpingUp:
        ctx.Physics.SetVertical(Math.Min(0, ctx.Physics.VerticalVelocity));
        break;

      case State.Falling:
        break;
    }
  }

  private bool IsCoyoteFalling(CharacterContext ctx) {
    if (ctx.Clock.TimeSince(_lastGroundedTime) < options.CoyoteTime) {
      return true;
    }

    return false;
  }

  private enum State {
    Idle,
    Walking,
    JumpingUp,
    Falling
  }
}
