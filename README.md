# 🏰 Dungeon Escape Room

> An atmospheric first-person escape room experience where exploration, observation and logical thinking are the keys to escaping the depths of the dungeon.

![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge\&logo=unity\&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-239120?style=for-the-badge\&logo=csharp\&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge\&logo=github\&logoColor=white)

---

## 🎮 About the Project

**Dungeon Escape Room** is a first-person puzzle game set deep inside a mysterious dungeon.

The player's goal is simple:

> **Find a way out.**

The dungeon is filled with interconnected rooms, hidden mechanisms, symbols, glyphs, locked doors and over **30 different puzzles** that must be solved in order to progress.

The game focuses heavily on **environmental interaction and logical problem solving**. Almost every element of the environment can potentially provide information, hide a clue or become part of a larger puzzle.

Rather than following a linear sequence of traditional puzzles, the player must observe the environment, understand the clues and connect seemingly unrelated elements together to discover the correct solution.

---

# 🏰 Gameplay

Exploration and puzzle solving are the core of the experience.

The player explores a multi-level dungeon consisting of rooms that differ in:

* size
* layout
* structure
* elevation
* accessibility
* puzzle design
* environmental details

Some rooms are simple and compact, while others contain multiple levels connected by stairs and different paths.

The environment itself becomes part of the puzzle.

| Feature                       | Description                                                        |
| ----------------------------- | ------------------------------------------------------------------ |
| 🧩 30+ Puzzles                | A large collection of logic and environmental puzzles              |
| 🏰 Dungeon Exploration        | Explore interconnected rooms throughout the dungeon                |
| 🔎 Environmental Clues        | Search the environment for information needed to progress          |
| 🗝️ Locked Doors              | Discover mechanisms and solutions required to open doors           |
| 🔣 Symbols & Glyphs           | Read and interpret mysterious symbols found throughout the dungeon |
| 🧠 Logic Puzzles              | Combine clues and information to discover solutions                |
| 🪜 Multi-Level Rooms          | Explore rooms with multiple floors and vertical paths              |
| 🖐️ Environmental Interaction | Interact with a wide range of objects and mechanisms               |
| 💡 Hints                      | Request hints when a puzzle becomes difficult                      |
| 🔔 Notifications              | Receive contextual information and gameplay feedback               |
| 🎮 Controller Support         | Full keyboard and gamepad interaction                              |

---

# 🧩 Puzzle System

Puzzles are the foundation of the game.

The project contains **over 30 different puzzles**, each designed around observation, interaction and logical reasoning.

Instead of simply searching for predefined items, the player often needs to understand how different elements of the environment relate to each other.

A solution might require:

1. discovering a symbol
2. finding where it is referenced
3. understanding its meaning
4. interacting with another object
5. combining the information
6. using the resulting solution to unlock the next part of the dungeon

This creates a gameplay loop where **information discovered in one part of the environment can become the key to solving something somewhere else**.

---

# 🔎 Environmental Interaction

One of the main technical focuses of the project is its extensive **environment interaction system**.

The player can interact with many different types of objects throughout the dungeon.

Examples include:

* doors
* mechanisms
* switches
* symbols
* glyphs
* puzzle elements
* interactive objects
* environmental clues
* other gameplay-related objects

The interaction system provides a common foundation for different gameplay mechanics while allowing individual objects to have their own behaviour.

---

# 🔣 Symbols & Glyphs

The dungeon contains various symbols and glyphs that provide information to the player.

These elements can act as:

* clues
* puzzle components
* references
* codes
* environmental storytelling elements
* parts of larger solutions

The player needs to carefully observe the environment and determine how these symbols relate to the surrounding puzzles.

Sometimes the information required to solve a puzzle may be located somewhere completely different from the object that needs to be interacted with.

---

# 🧠 Logic & Observation

The game focuses heavily on logical thinking rather than traditional combat-based gameplay.

The player is encouraged to:

* observe
* remember
* compare
* interpret
* experiment
* connect information
* test possible solutions

Many puzzles are designed so that the final solution emerges from combining multiple pieces of information discovered throughout the environment.

---

# 💡 Hint System

The game contains a dedicated hint system designed to help the player when they become stuck.

Hints provide additional information without directly removing the puzzle from the gameplay experience.

The system is designed around the idea of guiding the player toward discovering the solution rather than simply revealing it immediately.

---

# 🎮 Input System

The game supports both **keyboard & mouse** and **gamepad** controls.

| Input       | Support |
| ----------- | :-----: |
| ⌨️ Keyboard 🖱️ Mouse   |    ✅    |
| 🎮 Gamepad |    ✅    |

The interaction system is designed to work consistently regardless of the selected input method.

---

# 🧩 Interaction Architecture

The interaction system is designed to provide a common framework for different types of interactive objects.

Instead of every object implementing completely separate interaction logic, objects can expose their own interaction behaviour through the common system.

This allows the same interaction framework to be used for:

* opening doors
* activating mechanisms
* examining symbols
* triggering puzzle elements
* interacting with environmental objects
* progressing through the dungeon

This architecture also makes it easier to introduce new interactive objects without rebuilding the entire interaction system.

---

# 📋 Project Features

| System                        | Status |
| ----------------------------- | :----: |
| First Person Controller       |    ✅   |
| 30+ Puzzles                   |    ✅   |
| Logic Puzzles                 |    ✅   |
| Environmental Puzzles         |    ✅   |
| Advanced Interaction System   |    ✅   |
| Interactive Doors             |    ✅   |
| Interactive Mechanisms        |    ✅   |
| Symbols & Glyphs              |    ✅   |
| Multi-Level Rooms             |    ✅   |
| Stairs & Vertical Exploration |    ✅   |
| Hint System                   |    ✅   |
| Dungeon Exploration           |    ✅   |

---

# 🎯 Project Goals

The main goals of the project are:

* creating an immersive escape room experience
* developing a highly reusable interaction system
* designing a large number of interconnected puzzles
* creating environmental puzzles based on observation and logic
* implementing multi-level dungeon environments
* connecting puzzles with environmental interactions
* supporting both keyboard/mouse and gamepad controls
* creating a flexible hint and notification system
* developing systems capable of supporting different types of puzzle mechanics

---

# 🧠 Core Gameplay Loop

The game is built around a simple but interconnected gameplay loop:

```text
        EXPLORE
           ↓
        OBSERVE
           ↓
       FIND CLUES
           ↓
    UNDERSTAND SYMBOLS
           ↓
      SOLVE PUZZLE
           ↓
       ACTIVATE
       MECHANISM
           ↓
       OPEN PATH
           ↓
       EXPLORE MORE
           ↓
        ESCAPE
```

The environment itself is the main source of information.

Every room can contain something that contributes to a larger solution.

---

# 🚀 Project

**Dungeon Escape Room** is a Unity project focused on environmental puzzle design, interaction systems and logical problem solving.

The project combines a large number of interconnected puzzles with an extensive interaction system, multi-level dungeon environments and player assistance systems.

The main objective is simple:

> **Explore the dungeon. Understand its secrets. Solve the puzzles. Find the way out.**

**Engine:** Unity
**Language:** C#
**Genre:** Escape Room / Puzzle / Exploration
**Setting:** Dungeon
**Puzzles:** 30+
**Platforms:** Keyboard & Mouse / Gamepad
