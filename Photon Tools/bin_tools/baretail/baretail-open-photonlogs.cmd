@echo off
if NOT exist log\nul goto NOLOG
echo.Extract latest log files for Core and CLR
FOR /F %%I IN ('DIR log\Photon-*.* /B /O:D') DO SET NewestCoreLog=%%I
FOR /F %%I IN ('DIR log\PhotonCLR*.* /B /O:D') DO SET NewestCLRLog=%%I
start ..\bin_tools\baretail\baretail.exe log\%NewestCoreLog% log\%NewestCLRLog%
exit

:NOLOG
echo.No log files found. Please use "Open Logs" again after you started Photon.
pause
exit