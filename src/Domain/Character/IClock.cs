namespace Plantformer.Domain.Character;

public interface IClock {
  public float DeltaTime { get; }
  public float Now { get; }
  public ulong Frame { get; }
  public float TimeSince(float otherTime) => Now - otherTime;
}
