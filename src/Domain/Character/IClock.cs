namespace Plantformer.Domain.Character;

public interface IClock {
  public float DeltaTime { get; }
  public float Now { get; }
}
