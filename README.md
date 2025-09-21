This is a case project that aims for these 7 tasks to be fulfilled.

1) State Machine Refactoring
Currently, states like `MergeBlocksState` and `MoveBlocksState` directly handle both game logic and visual effects (DOTween animations, VFX calls, audio playback), creating several architectural issues. Design and implement an improved architecture that addresses these coupling issues. Your solution should achieve clean separation between game logic and visual presentation while maintaining all existing functionality.

2) Bomb Block
Introduce a new block type: Bomb. This block follows the same placement and movement rules as other blocks but introduces an explosive mechanic upon reaching its final position.

Behavior:
Players place the Bomb by tapping on a lane.
The Bomb moves down the grid until it collides with another block or reaches the top.
Upon stopping, the Bomb explodes, destroying all adjacent blocks.

Implementation Notes:
Ensure the explosion effect provides clear and impactful visual feedback.
Consider adding subtle screen shake and particle effects to enhance the player experience.
The destruction of blocks should integrate smoothly with the existing merge and movement mechanics.

3) Game Effects
Enhance core gameplay visual feedback on these areas:
Dynamic cell spawn animations
Smooth cell movement transitions
Impactful destination arrival effects
Engaging merge animations
Score feedback through:
Animated score counter updates
Floating score text animations
Visual emphasis on score milestones

4) End Game Panel
Design and implement engaging animations for both victory and defeat scenarios.
Create dynamic score display effects that reflect the game outcome.
Enhance visual feedback through for example:
Visual effects appropriate to the game result
Particle systems
Custom shaders
Develop asynchronous animation sequences for both outcomes, such as:
Result announcement animation (victory/defeat)
Themed particle effects
Final score reveal with appropriate styling
New game button presentation
Additional UI element animations

5) Scene Transitions
Design and implement smooth transitions between scenes.
Utilize Unity's additive scene loading for efficient panel management
Note: Complex navigation systems are not required as panels are scene-based

6) Asset Optimization
Implement sprite atlas packaging
Apply appropriate compression techniques
Organize elements into reusable prefabs

7) Main Menu
Add dynamic UI effects, such as:
Particle systems
UI animations
Tween effects
