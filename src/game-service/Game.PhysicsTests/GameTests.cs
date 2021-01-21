using Box2D.Window;
using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using static Box2DX.Dynamics.DebugDraw;

CreateSimulationWindow();

void CreateSimulationWindow()
{
    var window =
        new SimulationWindow(title: "Physics Tests", width: 800, height: 600)
        {
            VSync = OpenTK.VSyncMode.On
        };

    window.SetView(new CameraView());

    CreateMap(new DrawPhysics(window)
    {
        Flags = DrawFlags.Aabb | DrawFlags.Shape
    });

    window.Run(updateRate: 25.0);
}

void CreateMap(DrawPhysics drawPhysics)
{
    var gameSceneCollection = new ComponentCollection(new IComponent[]
    {
        new IdGenerator(),
        new GameSceneCollection(),
        new GameSceneManager()
    }).Get<IGameSceneCollection>();

    if (gameSceneCollection.TryGet(map: Map.TheDarkForest, out var gameScene))
    {
        gameScene.PhysicsWorldManager.SetDebugDraw(drawPhysics);

        CreatePlayer(gameScene);
    }
}

void CreatePlayer(IGameScene gameScene)
{
    var player = new PlayerGameObject(id: 1, new IComponent[]
    {
        new MessageSender(),
        new PlayerAttackedMessageSender()
    });
    var bodyData = player.CreateBodyData();

    gameScene.GameObjectCollection.Add(player);
    gameScene.PhysicsWorldManager.AddBody(bodyData);
}