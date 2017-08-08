@echo off
REM change dir to location of script
SET mypath=%~dp0
CD %mypath%

echo Stopping any running "application" Photon %1
start PhotonSocketServer.exe /stop
pause
