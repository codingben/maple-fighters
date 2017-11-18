from MathematicsHelper import *
from Physics.Box2D import PhysicsWorldInfo

sceneId = 2
sceneSize = Vector2(40, 5)
regionSize = Vector2(10, 5)

lowerBound = Vector2(-100, -100)
upperBound = Vector2(100, 100)
gravity = Vector2(0, -9.8)
doSleep = True
drawPhysics = True

physicsWorldInfo = PhysicsWorldInfo(lowerBound, upperBound, gravity, doSleep)

sceneContainer.AddScene(sceneId, sceneSize, regionSize, physicsWorldInfo, drawPhysics)