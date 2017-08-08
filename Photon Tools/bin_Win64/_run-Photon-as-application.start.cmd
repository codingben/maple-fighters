@echo off
REM change dir to location of script
SET mypath=%~dp0
CD %mypath%

REM check if photon is already running:
tasklist /fi "Imagename eq PhotonSocketServer.exe" > tasks.txt
::echo _tasklist - %errorlevel%
find "PhotonSocketServer.exe" tasks.txt
::echo _find - %errorlevel%
if %errorlevel% NEQ 1 goto ERROR

echo.
echo Starting Photon as application.
call PhotonSocketServer.exe /debug %1
::echo _start - %ERRORLEVEL%
goto END

:ERROR
echo.
echo Server already running

:END
del tasks.txt
pause
