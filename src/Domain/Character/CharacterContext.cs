namespace Plantformer.Domain.Character;

using Godot;

public record CharacterContext(IClock Clock, IInput Input, ICharacterPhysics Physics, ICharacterCombat Combat) {
  public void ApplyGravity(float g) {
    var acceleration = g * Clock.DeltaTime;
    Physics.Velocity += new Vector2(0, 1) * acceleration;
  }
}
