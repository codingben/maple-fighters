from MathematicsHelper import *
from Physics.Box2D.Core import PhysicsWorldInfo
from Game.Common import Maps

sceneSize = Vector2(30, 30)
regionSize = Vector2(10, 5)

lowerBound = Vector2(-100, -100)
upperBound = Vector2(100, 100)
gravity = Vector2(0, -9.8)
doSleep = False
drawPhysics = False

physicsWorldInfo = PhysicsWorldInfo(lowerBound, upperBound, gravity, doSleep)

sceneContainer.AddScene(Maps.Map_2, sceneSize, regionSize, physicsWorldInfo, drawPhysics)