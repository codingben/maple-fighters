from MathematicsHelper import *
from Game.InterestManagement import *
from Game.Application.SceneObjects import *

position = Vector2(-17.125, -5.5)
playerPosition = Vector2(-12.8, -12.8)
map = 3

portalSceneObject = Portal(position, playerPosition, map)

scene.AddSceneObject(portalSceneObject)