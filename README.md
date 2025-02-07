# MonoBird - Flappy Bird Clone

A Flappy Bird Clone that has been created using C# and Monogame Framework.

## Gameplay

<img src="https://github.com/user-attachments/assets/1c36bec6-261b-413c-8be7-48c9d0fcf076" alt="Loading..." width="500">

## Installation

The following are the instructions for running the code:

- Download the project as a ZIP file or clone it using GitHub Desktop.
- Open the project in Visual Studio or VS Code.
- Make sure you have C# installed.
  - If you are using Visual Studio, Install .NET desktop development.
  - If you are using VS Code, run the following command:
    
  ```
  code --install-extension ms-dotnettools.csharp
  ```
  
  - To verify installation run the following command:
    
  ```
  dotnet --version
  ```
  
- Install Monogame
  - If you are using Visual Studio you can install Monogame through Extensions window.
  - If you are using VS Code, you can run the following command to install Monogame Templates:
    
  ```
  dotnet new --install MonoGame.Templates.CSharp
  ```

- Run the code

## How to Use

The game only uses left mouse click as input. Press left click to start and jump.

## Code

Here is the brief explanation for what the classes are responsible for.  

- **Main.cs:** The code starts up from here. It responsible for loading data, updating, and drawing. You can change the screen size and running speed from here.

- **Input.cs:** Responsible for mouse and keyboard detection. The project used to have keyboard inputs for debugging.

- **Sounds.cs:** Class for loading and playing the sound effects.

- **Sprite.cs:** Class that stores the data for rendering sprites, including texture, position, rotation, scale, and effects.

- **Level.cs:** The class that binds all the other classes together. It also manages the fonts, screen effects, some sounds, and UI elements. 

- **Background.cs:** Class for drawing the ground and sky.

- **Pipe.cs:** Class that draws, controls and manages the pipes. In this class you can change the frequency, spacing, min and max Y positions of the pipes.

- **Bird.cs:** Class that manages bird's physics, collisions, controls, and animations. You can change elements like jumping strength, gravity, rotation speed, hitbox size. 
