namespace Plantformer.Utilities;

using System;
using Godot;

public static class RandomExtensions {
  public static Vector2 RandomInUnitSquare(this Random random) {
    return new Vector2(
      random.NextSingle(),
      random.NextSingle()
    );
  }
  public static Vector2 RandomOnUnitCircle(this Random random) {
    var angle = random.NextSingle() * MathF.PI * 2;
    return new Vector2(
      MathF.Cos(angle),
      MathF.Sin(angle)
    );
  }

  public static float Range(this Random random, float min, float max) {
    min = MathF.Min(min, max);
    max = MathF.Max(min, max);
    return random.NextSingle() * (max - min) + min;
  }
}
