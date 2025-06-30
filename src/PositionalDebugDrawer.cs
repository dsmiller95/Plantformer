namespace Plantformer;

using System.Collections.Generic;
using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Collections;
using Chickensoft.Introspection;
using Chickensoft.Log;
using Domain;
using Godot;
using Utilities;

public readonly record struct PositionalDebugDrawerSettings(bool DrawNames, bool DrawPath) {
  public static PositionalDebugDrawerSettings Default { get; } = new(true, true);
}

[Meta(typeof(IAutoNode))]
public partial class PositionalDebugDrawer: Node2D {
  public override void _Notification(int what) => this.Notify(what);

  [Export]
  public PackedScene? SegmentPrefab;
  [Export]
  public Node2D? SegmentParent;

  [Dependency] public IAutoProp<PositionalDebugDrawerSettings> Settings => this.DependOn<IAutoProp<PositionalDebugDrawerSettings>>(); // () => new AutoProp<PositionalDebugDrawerSettings>(PositionalDebugDrawerSettings.Default));

  private readonly List<PositionalDebugDrawerSegment> _segments = new();

  public void OnResolved() {
    Settings.Sync += SettingsChanged;
    _log.Info($"PositionalDebugDrawer resolved with settings: {Settings.Value}");
  }

  public void AppendDraw(DebugInfo debugPoint) {
    SegmentPrefab.AssertSetInInspector();
    SegmentParent.AssertSetInInspector();

	  var segment = _segments.LastOrDefault();
	  if (segment == null || segment.Info != debugPoint) {
	    segment = (PositionalDebugDrawerSegment)SegmentPrefab.Instantiate();
	    segment.Initialize(debugPoint);
      segment.ApplySettings(Settings.Value);
	    SegmentParent.GetParent().AddChild(segment);
	    _segments.Add(segment);
	  }

	  segment.AppendPosition(GlobalPosition);
  }

  private void SettingsChanged(PositionalDebugDrawerSettings settings) {
    _log.Info($"Settings changed: {settings}");
    foreach (var segment in _segments)
    {
      segment.ApplySettings(settings);
    }
  }

  protected override void Dispose(bool disposing) {
    if (disposing) {
      Settings.Changed -= SettingsChanged;
    }
    base.Dispose(disposing);
  }
  private readonly Log _log = new(nameof(PositionalDebugDrawer), new ConsoleWriter());
}
