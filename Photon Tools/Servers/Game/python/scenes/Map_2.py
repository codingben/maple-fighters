from MathematicsHelper import *

sceneId = 3
sceneSize = Vector2(30, 30)
regionSize = Vector2(10, 5)

sceneContainer.AddScene(sceneId, sceneSize, regionSize)

lowerBound = Vector2(-100, -100)
upperBound = Vector2(100, 100)
gravity = Vector2(0, -9.8)
doSleep = True
drawPhysics = True

sceneContainer.AddPhysics(sceneId, lowerBound, upperBound, gravity, doSleep, drawPhysics)