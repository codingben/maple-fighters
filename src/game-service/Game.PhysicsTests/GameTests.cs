/*
 * Test collision detection between the player game object
 * and another game object.
 */

using Box2DX.Dynamics;
using Box2D.Window;
using Game.Application.Components;
using Game.Application.Objects;
using Game.MessageTools;
using Game.Physics;
using static Box2DX.Dynamics.DebugDraw;

// Create a window to display physics simulation.
var window =
    new SimulationWindow(title: "Physics Tests", width: 800, height: 600)
    {
        VSync = OpenTK.VSyncMode.On
    };

window.SetView(new CameraView(position: OpenTK.Vector2.Zero, zoom: 0.008f));

// Create a game scene and physical world.
var collection = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new GameSceneCollection(),
    new GameSceneManager()
});

var gameSceneCollection = collection.Get<IGameSceneCollection>();
var mapName = Map.TheDarkForest.ToString().ToLower();
if (gameSceneCollection.TryGet(mapName, out var gameScene))
{
    gameScene.PhysicsWorldManager.SetDebugDraw(new DrawPhysics(window)
    {
        Flags = DrawFlags.Aabb | DrawFlags.Shape
    });

    var player = new PlayerGameObject(id: 1, name: "Player");
    player.Transform.SetPosition(new InterestManagement.Vector2(10, 5));

    var bodyDef = new BodyDef();
    var x = player.Transform.Position.X;
    var y = player.Transform.Position.Y;
    bodyDef.Position.Set(x, y);
    bodyDef.UserData = player;

    var polygonDef = new PolygonDef();
    polygonDef.SetAsBox(0.3625f, 0.825f);
    polygonDef.Density = 0.1f;
    polygonDef.Filter = new FilterData()
    {
        GroupIndex = (short)LayerMask.Player
    };

    var bodyData = new NewBodyData(player.Id, bodyDef, polygonDef);
    gameScene.GameObjectCollection.Add(player);
    gameScene.PhysicsWorldManager.AddBody(bodyData);
}

window.Run(updateRate: 25.0);