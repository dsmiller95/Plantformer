using System.Collections.Generic;

namespace Plantformer.Domain.Entities;

public class World {
  public Dictionary<EntityId, IEntity> Entities { get; } = new();
}
