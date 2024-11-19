# A* Pathfinding System in Unity 🗺️🔍
This repository contains a Pathfinding System developed in Unity. The project demonstrates efficient pathfinding algorithms and tools, perfect for AI navigation, obstacle avoidance, and dynamic grid-based systems in 2D and 3D games.

## Project Overview
This system generates a navigable grid and implements an optimized pathfinding algorithm to move AI-controlled entities or the player between specified points. It includes visualization tools and customizability for grid creation and obstacle placement.

## Features
### Pathfinding Mechanics
- Grid-Based Pathfinding:
  - A flexible, node-based grid system (GridGenerator.cs and Node.cs) supports dynamic terrain generation.
- AI Movement:
  - Enemy AI (EnemyAI.cs) dynamically navigates the environment using generated paths (PathGenerator.cs).
- Player Movement:
  - Smooth, grid-based player controls are handled by PlayerMovement.cs.

### Obstacle Management
- Dynamic Obstacles:
  - Obstacles are generated procedurally using ObstacleGenerator.cs and can influence pathfinding in real-time.
- Interactive Tiles:
  - Grid tiles (Tile.cs) can be customized for different terrains or effects.

### Grid Customization
- Grid Data Scriptable Objects:
  - Easily configure grid size and properties using GridDataSO.cs.
- Tile Visualization:
  - Tiles can be rendered with different visual states using TileVisual.cs.

### Game Feel Enhancements
- Coordinate Display:
  - Real-time UI updates (CordinatesUI.cs) show node details, enhancing debugging and clarity.
- Custom Editor Tools:
  - Editor scripts allow easy adjustment of grid settings (EditorWindow).

### Optimized Performance
- Heap Data Structure:
  - A custom implementation (Heap.cs) ensures efficient pathfinding operations with minimal overhead.
- Dynamic Path Updates:
  - Paths adapt to environmental changes, ensuring accuracy and reliability.

![Screenshot 2024-11-20 003039](https://github.com/user-attachments/assets/1daed2ec-5405-4307-a0ac-d84759286264)
![Screenshot 2024-11-20 003045](https://github.com/user-attachments/assets/04af53cc-b595-429d-ae03-2e4246b6231d)
