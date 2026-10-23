# Unity editor setup for the new features

All scripts are in `Assets/_Scripts`. Open the **Play** scene and do these steps once.

## 1. Player
`PlayerTouchController.cs` was renamed to `PlayerController.cs` so the file name matches the class (Unity can't attach it otherwise). The script had fallen off the Player in the scene.

1. Select **Player** (tag is already `Player`).
2. Add Component → **PlayerController**.
3. Optional: drag `sfx_laser1` into **Shoot Sound**, and a child empty object into **Fire Point** (e.g. at y = 0.6).

## 2. Bullet prefab
1. Drag a laser sprite (e.g. `PNG/Lasers/laserBlue01`) into the Hierarchy and name it **Bullet**.
2. Set its Sorting Layer / Order so it draws above the background (same as the Player).
3. Add **BoxCollider2D** → tick **Is Trigger**.
4. Add **Rigidbody2D** → Body Type **Kinematic**.
5. Add **BulletBehavior** (Speed 12, Top Boundary 8).
6. Drag it into `Assets/Prefabs` to make a prefab, then delete it from the Hierarchy.

## 3. Bullet pool + factory
1. Create an empty GameObject named **BulletManager**.
2. Add **BulletManager** (it adds **BulletFactory** automatically).
3. Drag the **Bullet** prefab into BulletFactory → **Bullet Prefab**. Pool Size 20 is fine.

## 4. Lives and score
1. Under **Canvas**, create two **UI → Text - TextMeshPro** objects: **LivesText** (top-left) and **ScoreText** (top-right).
2. Create an empty GameObject named **GameManager**, add **GameManager**.
3. Drag LivesText and ScoreText into the matching fields. Max Lives 3.
4. Optional: `sfx_shieldDown` for **Hit Sound**, `sfx_lose` for **Lose Sound**.

## 5. Obstacle prefab
Open `Assets/Prefabs/Obstacle`. The **ObstacleBehavior** component now has **Point Value** (default 10). Nothing else to change.

## How it works
- **Lives:** `ObstacleBehavior` calls `GameManager.LoseLife()` when it touches the Player. There's 1 second of invincibility after a hit. At 0 lives, the score is saved to `PlayerPrefs("FinalScore")` and the End scene loads.
- **Pooled bullets + factory:** `BulletFactory.CreateBullet()` is the only place bullets are instantiated. `BulletManager` fills a `Queue` with them at start. `GetBullet()` takes one from the queue (or asks the factory for a new one if the queue is empty), and `ReturnBullet()` puts it back.
- **Bullet vs enemy:** `BulletBehavior.OnTriggerEnter2D` checks for `ObstacleBehavior`. On a hit it adds that obstacle's points, turns the obstacle off (it goes back to the obstacle pool) and returns the bullet to its pool.
- **Points:** `GameManager.AddScore()` updates the score text.
