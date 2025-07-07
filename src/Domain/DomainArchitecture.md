
# Responsibility segregation=


- domain
  - Actions
  - Input events -> internal state -> output events
- bindings
  - Interaction access (hitbox query)
  - syncing domain with physics system
    - physics velocity
    - IsGrounded
    - optionally position
      - position is often not necessary
  - Display syncing
    - facing direcdtion -> sprite flips, hitbox flips, etc.
  - Input mapping
    - press right -> map to Domain.RightMove action
- engine (Godot)
  - physics simulation
    - moveAndSlide
    - collision queries
    - hitbox/hurtbox queries and etc

| Interaction | Domain                           | Bindings                                                 | Engine (godot)                   |
|------------|----------------------------------|----------------------------------------------------------|----------------------------------|
| Movement   | domain Input -> Velocity Δ | input key -> domain input<br/>physics vel <-> domain vel | keyboard input<br/>Physics simulation |


## Movement

```mermaid
sequenceDiagram
  participant Godot
  participant Bindings
  participant Domain
  Bindings->>Godot: Input.keyDown(d)
  Godot-->>Bindings: true
  Bindings->>Godot: Physics.Velocity
  Godot-->>Bindings: Velocity(0, 0)
  Bindings->>Domain: Tick(Context(InputEvent(MoveRight), (0, 0)))
  activate Domain
  Domain-->>Bindings: OutputEvent(Velocity(1, 0))
  deactivate Domain
  Bindings->>Godot: Physics.Velocity = (1, 0)
```

## Combat


```mermaid
sequenceDiagram
  participant Godot
  participant Bindings
  participant Domain
  Bindings->>Godot: Input.keyDown(space)
  Godot-->>Bindings: true
  Bindings->>Domain: Tick(Context(InputEvent(Attack), (0, 0)))
  activate Domain
  Domain->>Bindings: Query(Hitbox(Punch))
  Bindings ->>Godot: Hitbox.Punch.GetTargets()
  Godot-->>Bindings: [Target_A, Target_B]
  Bindings-->>Domain: [Target_A, Target_B]
  Domain-->>Bindings: OutputEvent(Target_A, ImpulseForce(100, 0))
  deactivate Domain
  Bindings->>Godot: Target_A.Physics.Impuslse(100, 0)
```
