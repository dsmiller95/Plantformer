namespace Plantformer;

using Chickensoft.AutoInject;
using Chickensoft.Collections;
using Chickensoft.Introspection;
using Chickensoft.Log;
using Godot;
using Utilities;

[Meta(typeof(IAutoNode))]
public partial class GlobalConfig: Node, IProvide<IAutoProp<PositionalDebugDrawerSettings>> {
  public override void _Notification(int what) => this.Notify(what);

  [Export]
  public bool DrawNames = true;
  [Export]
  public bool DrawPath = true;

  private readonly AutoProp<PositionalDebugDrawerSettings> _debugDrawSettings =
    new(PositionalDebugDrawerSettings.Default);

  IAutoProp<PositionalDebugDrawerSettings> IProvide<IAutoProp<PositionalDebugDrawerSettings>>.Value() =>
    _debugDrawSettings;

  // Call the this.Provide() method once your dependencies have been initialized.
  public void OnReady() {
    var newSettings = new PositionalDebugDrawerSettings(DrawNames, DrawPath);
    _debugDrawSettings.OnNext(newSettings);
    _log.Info($"GlobalConfig ready: {newSettings}");
    this.Provide();
  }

  public void OnProvided() {
    // You can optionally implement this method. It gets called once you call
    // this.Provide() to inform AutoInject that the provided values are now
    // available.
  }
  private readonly Log _log = new(nameof(GlobalConfig), new ConsoleWriter());
}
