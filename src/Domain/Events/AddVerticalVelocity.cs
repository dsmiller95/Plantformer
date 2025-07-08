namespace Plantformer.Domain.Events;

using ExhaustiveMatching;

public record AddVerticalVelocity(float Vertical) : IOutputEvent;

[Closed(typeof(AddVerticalVelocity))]
public partial interface IOutputEvent;
