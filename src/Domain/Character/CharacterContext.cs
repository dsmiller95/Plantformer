namespace Plantformer.Domain.Character;

public record CharacterContext(IClock Clock, IInput Input, ICharacterPhysics Physics) {
  public void ApplyGravity(float g) {
    var acceleration = g * Clock.DeltaTime;
    Physics.SetVertical(Physics.VerticalVelocity + acceleration);
  }
}
