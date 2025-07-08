namespace Plantformer.Domain.Character;

using Events;

public record CharacterContext(IClock Clock, IInput Input, ICharacterPhysics Physics, ICharacterCombat Combat, IEventSink eventSink) {
  public void ApplyGravity(float g) {
    var acceleration = g * Clock.DeltaTime;
    eventSink.AppendEvent(new AddVerticalVelocity(acceleration));
  }

  public void SetHorizontalAndFacing(float vx) {
    eventSink.AppendEvent(new SetHorizontalVelocity(vx, true));
  }

  public void SetVerticalVelocity(float vy) {
    eventSink.AppendEvent(new SetVerticalVelocity(vy));
  }
}

