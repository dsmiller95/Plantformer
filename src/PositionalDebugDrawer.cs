namespace Plantformer;

using System.Collections.Generic;
using System.Linq;
using Domain;
using Godot;

public partial class PositionalDebugDrawer: Node2D {
  [Export]
  public required PackedScene SegmentPrefab;
  [Export]
  public required Node2D SegmentParent;

  private readonly List<PositionalDebugDrawerSegment> _segments = new();

  public void AppendDraw(DebugInfo debugPoint) {

    var segment = _segments.LastOrDefault();
    if (segment == null || segment.Info != debugPoint) {
      segment = (PositionalDebugDrawerSegment)SegmentPrefab.Instantiate();
      segment.Initialize(debugPoint);
      SegmentParent.GetParent().AddChild(segment);
      _segments.Add(segment);
    }

    segment.AppendPosition(GlobalPosition);
  }
}
