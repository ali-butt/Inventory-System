# 🐾 Small Animal Inventory System – Unity Task

This project is designed to demonstrate inventory management demo.

---

## 📦 Features

- ✅ **ScriptableObject-based Animal Shop**  
  All animals are defined as ScriptableObjects, providing modular, editable definitions for species, stats, and icons.

- ✅ **Inventory System with JSON + PlayerPrefs**  
  Owned animals are saved using runtime `AnimalInstance` objects, serialized to JSON and stored in `PlayerPrefs`.

- ✅ **Star Currency System**  
  Players can buy animals using stars. Purchases are validated and stars are deducted accordingly.

- ✅ **Two Screens (Shop & Inventory)**

  - **Shop**: Displays available animals not yet owned
  - **Inventory**: Displays purchased animals  
    Manual buttons are used to toggle between screens.

- ✅ **Clean & Modular Codebase**  
  Scripts follow single-responsibility and separation-of-concerns principles. All data logic is decoupled from UI.

- ✅ **Basic Error Handling**  
  Handles corrupted or missing JSON and safely initializes fallback data.

---

## 🛠 Setup Instructions

1. Clone or unzip the Unity project.
2. Open in Unity (recommended version: **Unity 2021.3 LTS or newer**).
3. Open the `Main` scene.
4. Enter Play Mode.

---

## 🧪 Usage Guide

### 🎮 Buying Animals:

- Click the **Shop** button to view all available animals.
- Click **Buy** on any animal to purchase it using stars.

### 🎒 Viewing Inventory:

- Click the **Inventory** button to view all owned animals.
- Click **Remove** to remove an animal and (optionally) refund stars.

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Managers/          ← ShopManager, InventoryManager, StarManager
│   ├── UI/                ← Shop UI and Inventory UI scripts
│   ├── Data/              ← ScriptableObjects and AnimalInstance class
├── Resources/
│   └── Animals/           ← All AnimalDataSO assets used by the shop
├── Prefabs/
│   └── UI/                ← AnimalCard prefabs for shop & inventory
├── Scenes/
│   └── Main.unity         ← Entry point scene
```

---

## 📋 Notes

- UI switching is done manually via Unity buttons (not script-based).
- Assets (sprites, fonts, buttons) provided in the task were used where possible.
- Designed for extensibility (adding more animals or stats is simple).

---

## 🙋‍♂️ Author

**Ali Iftikhar Butt**  
Unity Developer | [aliiftikharbutt@gmail.com]
