namespace Plantformer.Domain.Character;

using System.Collections.Generic;
using Events;

public interface IEventSink {
  public void AppendEvent(IOutputEvent outputEvent);
}

public class ListEventSink : IEventSink {
  private readonly List<IOutputEvent> _events = new();

  public void AppendEvent(IOutputEvent outputEvent) {
    _events.Add(outputEvent);
  }

  public IReadOnlyList<IOutputEvent> Events => _events;
}
