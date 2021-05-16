/*
 * The test here is to ensure that the collision detection works correctly.
 */

using Box2D.Window;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.MessageTools;
using OpenTK;
using static Box2DX.Dynamics.DebugDraw;

// Create a window to display physics simulation.
var window =
    new SimulationWindow(title: "Physics Tests", width: 800, height: 600)
    {
        VSync = OpenTK.VSyncMode.On
    };

window.SetView(new CameraView(position: Vector2.Zero, zoom: 0.008f));

// Create a game scene and physical world.
var collection = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new GameSceneCollection(),
    new GameSceneManager()
});

var gameSceneCollection = collection.Get<IGameSceneCollection>();
if (gameSceneCollection.TryGet(map: Map.TheDarkForest, out var gameScene))
{
    gameScene.PhysicsWorldManager.SetDebugDraw(new DrawPhysics(window)
    {
        Flags = DrawFlags.Aabb | DrawFlags.Shape
    });

    // Create a player game object.
    var player = new PlayerGameObject(id: 1, new IComponent[]
    {
        new MessageSender(new NativeJsonSerializer()),
        new PlayerAttackedMessageSender()
    });
    var bodyData = player.CreateBodyData();

    // Add a player and his physical body.
    gameScene.GameObjectCollection.Add(player);
    gameScene.PhysicsWorldManager.AddBody(bodyData);
}

window.Run(updateRate: 25.0);