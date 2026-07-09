# Island Escape (Core Gameplay Scripts)

A third-person, over-the-shoulder adventure game built in Unity. Players control a sibling surviving a plane crash on a remote island while trying to escape. 

## 🎮 Game Concept & Premise
After a plane crash leaves a brother and sister stranded as the sole survivors, the objective is to scour the map, locate 5 hidden pieces of a broken boat, and escape the island. 

* **Dynamic Threat Level:** A predatory jaguar stalks the player across the map. The jaguar remains passive until the first boat piece is retrieved, and cannot be permanently defeated until the final piece is secured.
* **First-Boot Customization:** A dedicated character customization screen triggers on initial launch, locking into a persistent game state while remaining accessible via in-game menus later.
* **Persistent Settings:** Audio configurations (Music/SFX toggles) utilize a persistent saving system that maintains user preferences across game restarts.

## 🛠️ Technical Implementation (C# Architecture)
This repository hosts the core architectural scripts handling the game's modular logic:
* **`PlayerController.cs` & `ThirdPersonController.cs`:** Manages over-the-shoulder camera orientation, movement inputs, and environmental traversal.
* **`AudioManager.cs`:** Drives environmental music and tactical SFX toggles alongside a persistent saving system.
* **`BoatManager.cs` & `BoatPiece.cs`:** Tracks game progression, objective tracking, and handles the state changes for target items.
* **`CharacterSwapManager.cs` & `CustomizationManager.cs`:** Manages character parameters and initial launch ui state initialization.
* **`UIManager.cs` & `MenuPanelController.cs`:** Powers the responsive front-end menu hierarchy and options navigation.

## 🚀 Target Platforms & Status
* **Status:** Active Development (Core systems mapped; asset refinement, cutscenes, and survival health systems in progress).
* **Target Platforms:** Windows, Mac, Linux, and Android.
