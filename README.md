# nlcEngine
A Game Engine to build a 3d and 2d games in .NET.

# Overview

#### nlcEngine includes:
- *Flexible drawing pipeline*
- *Easy initialization methods*
- *The API that never requires the deep knowledge of 3D Graphics Math*

# Runtime
.NET 7+

# Installation
This project is currently in the progress of development.

# Getting started
### Creating An Empty Window
```C#
NlcEngineGame.Init( new Profile( 640, 480, 640, 480, "My First Game on NlcEngine!", 60.0 ) ); // 640x480 window, with 60.0 fps
NlcEngineGame.Start();  // Start the game
```

### Adding A Scene
```C#
class MyScene : Scene
{
  public override void OnUserLoad()
  {
    // Scene loading code here
    BackgroundColor = Color.SkyBlue;  // Set the background to sky blue
  }
  
  public override void OnUserUpdate(float elapsedTime)
  {
    // Update code
  }
  
  public override void OnUserRender(float elapsedTime)
  {
    // Render code
  }
}
```
Set the current scene:
```C#
//NlcEngineGame.Init( new Profile( /* ... */ ) );
NlcEngineGame.SceneService.Navigate( new MyScene() );  // Navigate to MyScene
//NlcEngineGame.Start();
```
