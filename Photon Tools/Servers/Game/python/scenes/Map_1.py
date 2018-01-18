from MathematicsHelper import *
from Physics.Box2D import PhysicsWorldInfo
from Shared.Game.Common import Maps

sceneSize = Vector2(40, 5)
regionSize = Vector2(10, 5)

lowerBound = Vector2(-100, -100)
upperBound = Vector2(100, 100)
gravity = Vector2(0, -9.8)
doSleep = True
drawPhysics = False

physicsWorldInfo = PhysicsWorldInfo(lowerBound, upperBound, gravity, doSleep)

sceneContainer.AddScene(Maps.Map_1, sceneSize, regionSize, physicsWorldInfo, drawPhysics)