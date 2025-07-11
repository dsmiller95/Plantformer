﻿namespace Plantformer;

using System.Collections.Generic;
using Domain;
using Godot;

public partial class PositionalDebugDrawerSegment: Node2D {
  public DebugInfo Info { get; private set; } = new(Colors.White, "Default");
  public PositionalDebugDrawerSettings Settings { get; private set; } = PositionalDebugDrawerSettings.Default;
  private readonly List<Vector2> _positions = new();

  public void Initialize(DebugInfo info) {
    Info = info;
  }

  public void AppendPosition(Vector2 position) {
    _positions.Add(position);
    QueueRedraw();
  }

  public void ApplySettings(PositionalDebugDrawerSettings settings) {
    Settings = settings;
    QueueRedraw();
  }

  public override void _Draw() {
    if (_positions.Count < 2) {
      return;
    }

    if (Settings.DrawPath) {
      var color = Info.Color;
      for (var i = 0; i < _positions.Count - 1; i++) {
        DrawLine(_positions[i], _positions[i + 1], color, 2f);
      }
    }

    if (Settings.DrawNames) {
      var debugIndex = _positions.Count / 2;
      DrawString(ThemeDB.FallbackFont, _positions[debugIndex], Info.Description, HorizontalAlignment.Center);
    }
  }
}
