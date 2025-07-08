namespace Plantformer.Domain.Events;

using ExhaustiveMatching;

public record SetVerticalVelocity(float Vertical) : IOutputEvent;

[Closed(typeof(SetVerticalVelocity))]
public partial interface IOutputEvent;
