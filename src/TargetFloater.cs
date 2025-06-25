namespace Plantformer;

using Godot;
using Utilities;

public partial class TargetFloater : RigidBody2D
{
  public override void _Ready() {
    base._Ready();
    var rng = new System.Random((int)new RandomNumberGenerator().GetSeed());
    this.ApplyImpulse(rng.RandomOnUnitCircle() * 100f);
    this.ApplyTorqueImpulse(rng.Range(-100, 100));
  }
}
