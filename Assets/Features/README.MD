# Code base design

RULE OF THUMB:
Ask this: "Is this something that belongs only to the player, or does it interact with the game world?"

If player-specific only → keep it inside PlayerService.

If shared, global, or system-wide → move it to its own service or manager.

Principles You’re Using


1. Interface Segregation Principle (ISP)
“Clients should not be forced to depend upon interfaces that they do not use.”

We're defining specific interfaces like IEconomyService, IExpService, etc.

Each consumer (e.g., UI, game logic) only uses what it needs, not the whole PlayerService.

2. Dependency Inversion Principle (DIP)
“Depend on abstractions, not concretions.”

You inject interfaces (like IEconomyService) instead of using concrete EconomyService.

This allows for easier testing, swapping, and extending.


```csharp
public class UIManager
{
    private readonly IEconomyService economy;

    public UIManager(IEconomyService economy)
    {
        this.economy = economy;
    }
}
```
3. Single Responsibility Principle (SRP)
“A class should have only one reason to change.”

You’ve separated concerns: EconomyService, ExpService, HealthService, each handling one domain.

PlayerService acts as an orchestrator, not a god-object.

4. Encapsulation
Hiding internal state and exposing only what’s necessary.

You’re not exposing raw data (PlayerData) directly — instead, you wrap it in a service.

Consumers use method calls like GetRequiredCoinsToLevelup() rather than reading fields directly.

5. Open/Closed Principle (OCP)
“Software entities should be open for extension, but closed for modification.”

Your use of interfaces allows you to extend functionality (e.g., replace or add new IEconomyService) without changing existing code.

6. Law of Demeter (Principle of Least Knowledge)
“A class should only talk to its immediate friends.”

Your UI code doesn’t dig into PlayerData → Stats → Coins — it just calls GetRequiredCoinsToLevelup().

✨ Bonus Patterns We're Touching
Service Pattern — You’ve wrapped domain logic into reusable services.

Observer Pattern / Event system — We're using onDamage, etc., to notify listeners like UI.

Dependency Injection Friendly — You’ve structured your system so it can support DI containers if you want to adopt them later.

## Summary

“The system follows SOLID principles, especially Interface Segregation and Dependency Inversion. Each domain feature is encapsulated in a service, exposed via interfaces, and events are used to decouple communication. It’s service-oriented and testable by design.”
