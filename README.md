# Zenith Framework

Welcome to the **Zenith Framework**! Based on *Clean Architecture*, *Clean Code*, *Game Programming Patterns* and industry experience, this Unity framework is designed to help new and experienced developers start their projects faster while ensuring scalability and reliability as the codebase grows.

---

## Introduction

### Why Use Zenith Framework?

When developing medium-to-large-scale Unity projects, maintaining clean, modular, and scalable code becomes challenging. The Zenith Framework is built on principles derived from *Clean Architecture* to solve this problem, ensuring:

- **Rapid Development:** Start your projects with a solid architecture and **ready-to-use systems**, reducing the setup time.
- **Maintainability:** Easily locate and fix bugs.
- **Scalability:** Expand your game systems and add new features without breaking existing functionality.
- **Reusability:** Create reusable components that can speed up future projects.

The framework utilizes **Component** and **ServiceLocator** architectures to keep your code decoupled and flexible. It emphasizes creating modular systems, enabling developers to replace or extend functionality without widespread code changes.

---

## Core Principles

Zenith Framework is grounded in these three architectural principles:

1. **Common Closure Principle**:
   - Group classes that change for the same reason to simplify maintenance.

2. **Common Reuse Principle**:
   - Avoid dependencies on classes you don't use.

3. **Reuse/Release Equivalence Principle**:
   - Group classes to maximize reusability and minimize unnecessary coupling.

These principles ensure that your code remains clean, adaptable, and aligned with industry standards.

![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/CleanArchitectureBook.png?raw=true)

> *Reference:* "Clean Architecture: A Craftsman's Guide to Software Structure and Design" by Robert C. Martin.

### Tip:
Treat the Zenith Framework like a tool—test it thoroughly before fully integrating it into your project. A long-term relationship starts with a few good dates! 😉

---

## Key Concepts and Components

### 1. Service
A **Service** is a core system that provides functionality widely used across your game. Examples include:

- Object Pooling
- VFX Spawner
- Time Manager
- Screen Manager

**Why Services?**
Services are accessed via the **ServiceLocator** and use interfaces to enforce limited, controlled access. This design ensures that:

- Complex logic can be refactored without affecting dependent code.
- Systems remain decoupled for easier maintainability, testing and bugfixing.
- Replacement services can be easily swapped in with minimal changes.

**Example:** Switching from Asset Bundles to Addressables in your Object Pooling system requires only creating a new implementation of the same interface.

Before refactor:


![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/Services.png?raw=true)


After refactor:


![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/ServicesNewImplementation.png?raw=true)

---

### 2. Entity
A **Entity** is a MonoBehaviour that manages events, data, and states within a specific context. It serves as the bridge between the **Services** and **Components**.

Entitys should:
- Hold state information relevant to a feature or system.
- Act as a hub for other **Components** to update or retrieve state data.


General application example:

![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/GenericApplication.png?raw=true)

---

### 3. Component
A **Component** implements the primary logic of a feature while interacting with Services or Entitys. Components are designed to:

- Focus on a single feature per class.
- Be reusable across different systems or contexts.

**Relationships with Entitys:**
1. Update the Entity's state when necessary.
2. Implement the features associated with the Entity.

![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/EntityComponentRelation.png?raw=true)

This separation ensures that changes to a specific feature only affect the associated Component, leaving Entitys and Services intact.

**Example:** A `HealthComponent` manages the health logic, while the `HealthEntity` tracks and communicates health states to other systems.

---

### 4. Event Service
The **Event Service** enables decoupled communication between Components and Entitys, ensuring they remain independent and reusable. 


![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/EventServiceExample.png?raw=true)


#### How to Use the Event Service:
1. **Create Your Event Class:**
   - Create a script with the name of your event (e.g., `EnableInputsEvent`) in your `Events` folder.
   - Make this script inherit from `GameEventBase`.

```csharp
public class EnableInputsEvent : GameEventBase
{
   public bool Enable { get; private set; }

   public EnableInputsEvent(bool enable)
   {
      Enable = enable;
   }
}
   ```
   
2. **Subscribe to the event:**

   - Use the IEventService.AddEventListener method to subscribe to your event (in this case: EnableInputsEvent). Ensure to pass your method and a unique hash code.
  ```csharp
public class CombatentAIComponent : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
   
    private bool _isEnabled = false;
    private IEventService _eventService;

    private async void Awake()
    {
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.AddListener<EnableInputsEvent>(HandleEnableInputs, GetHashCode());
    }

    private void HandleEnableInputs(EnableInputsEvent inputEvent)
    {
        if (inputEvent.Enable)
        {
            // Implementation here...
        }
        else
        {
            // Different implementation here...
        }
    }
}
```

3. **Invoke the event with:**

  - IEventService.TryInvokeEvent(new YourEvent(data));
  
```csharp
public class GameSystemComponent : MonoBehaviour
{
   private bool _fightIsOn = false;

   private IEventService _eventService;

   private void Awake()
   {
      StartMatch();
   }

    private async void StartMatch()
    {
        await Task.Delay(1000);
        _eventService = await ServiceLocator.GetService<IEventService>();
        _eventService.TryInvokeEvent(new EnableInputsEvent(true));
        _fightIsOn = true;
    }
}
  ```

4. **You can unsubscribe at any time.**
   - To prevent memory leaks or unexpected behavior, unsubscribe from events when they're no longer needed.

  
   ```csharp
   public class CombatentAiComponent : MonoBehaviour
   ...
   private void OnDestroy()
    {
        _eventService.RemoveListener<EnableInputsEvent>(GetHashCode());
    }
   ```

   Example Use Case: Notify all relevant systems when the game starts without creating direct dependencies between them.

---

#### Additional Services
The Zenith Framework includes additional ready-to-use services such as:

**Object Pooling**: Efficiently reuse objects to reduce memory allocations and improve performance.
**More Services Coming Soon!** (Stay tuned for additional services and documentation.)

---
#### **Download the Framework Using UPM(Recommended)**

1. Open the Unity Package Manager.
2. Select the 'Add package from Git URL' option.
4. Paste  *https://github.com/gabrielgborges/zenith-framework.git*

---

#### **Add the Framework by Modifying the manifest.json File**

1. Navigate to the Packages folder in your Unity project directory and open the manifest.json file using a text editor or IDE.
2. Insert *"gabi.zenith.framework": "https://github.com/gabrielgborges/zenith-framework.git"* in the "dependencies" section of the manifest.json file.
3. Save the changes to the manifest.json file and return to Unity, it will automatically download and integrate the Zenith Framework package.

![alt text](https://github.com/gabrielgborges/zenith-framework/blob/main/DOCUMENTATION/AddPackageByManifest.mp4?raw=true)

Additional Notes:
Ensure that Git is installed on your system because Unity requires Git to fetch packages directly from GitHub.

---


#### **Getting Started**
Follow these steps to integrate the Zenith Framework into your Unity project:

1. **Download the Framework:**

   Clone the repository and paste in your project or include it using the UPM method above.

2. **Set Up Services:**

   Use the ServiceLocator to register and access your custom services.

3. **Use Entitys and Components:**

   Leverage Entitys for managing state and Components for implementing features.

4. **Integrate Event-Driven Architecture:**

   Utilize the Event Service to enable decoupled communication between systems.

---

#### Contributing
Contributions to the Zenith Framework are welcome! If you encounter issues or have ideas for new features, feel free to:

Submit a pull request.
Open an issue on GitHub.

---

#### License
This project is licensed under the MIT License. See the LICENSE file for details.
