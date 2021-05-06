using Box2D.Window;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.MessageTools;
using static Box2DX.Dynamics.DebugDraw;

var window =
    new SimulationWindow(title: "Physics Tests", width: 800, height: 600)
    {
        VSync = OpenTK.VSyncMode.On
    };

window.SetView(new CameraView());

var gameSceneCollection = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new GameSceneCollection(),
    new GameSceneManager()
})
.Get<IGameSceneCollection>();

if (gameSceneCollection.TryGet(map: Map.TheDarkForest, out var gameScene))
{
    gameScene.PhysicsWorldManager.SetDebugDraw(new DrawPhysics(window)
    {
        Flags = DrawFlags.Aabb | DrawFlags.Shape
    });

    var player = new PlayerGameObject(id: 1, new IComponent[]
    {
        new MessageSender(new NativeJsonSerializer()),
        new PlayerAttackedMessageSender()
    });
    var bodyData = player.CreateBodyData();

    gameScene.GameObjectCollection.Add(player);
    gameScene.PhysicsWorldManager.AddBody(bodyData);
}

window.Run(updateRate: 25.0);