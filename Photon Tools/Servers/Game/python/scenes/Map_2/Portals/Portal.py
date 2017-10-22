from MathematicsHelper import *
from Game.InterestManagement import *
from Game.Application.SceneObjects import *

position = Vector2(12.5, -1.125)
playerPosition = Vector2(18, -6)
map = 2

portalSceneObject = Portal(position, playerPosition, map)

scene.AddSceneObject(portalSceneObject)