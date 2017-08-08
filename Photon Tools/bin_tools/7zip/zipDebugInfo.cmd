@echo off
:: working directory is PhotonControl directory (when started from PhotonControl)

if not exist oldDebugInfo mkdir oldDebugInfo
move debugInfo_*.7z oldDebugInfo\.
move debugInfo*.txt oldDebugInfo\.

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: get time and date (somewhat) locale independent
:: http://stackoverflow.com/questions/203090/how-to-get-current-datetime-on-windows-command-line-in-a-suitable-format-for-usi
echo.Get timestamp
:: time
For /f "tokens=1-2 delims=/:" %%a in ("%TIME%") do (set mytime=%%a%%b)
:: date (locale indepentend)
SETLOCAL ENABLEEXTENSIONS
if "%date%A" LSS "A" (set toks=1-3) else (set toks=2-4)
for /f "tokens=2-4 delims=(-)" %%a in ('echo:^|date') do (
  for /f "tokens=%toks% delims=.-/ " %%i in ('date/t') do (
    set '%%a'=%%i
    set '%%b'=%%j
    set '%%c'=%%k))
if %'yy'% LSS 100 set 'yy'=20%'yy'%
set Today=%'yy'%-%'mm'%-%'dd'% 
ENDLOCAL & SET v_year=%'yy'%& SET v_month=%'mm'%& SET v_day=%'dd'%

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: create file with debug info
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo.Create debug info file
if exist debugInfo.txt del debugInfo.txt
echo date: %V_Year%-%V_Month%-%V_Day% time: %time% >> debugInfo.txt
ver >> debugInfo.txt
Setlocal
:: Get windows Version numbers
:: http://stackoverflow.com/questions/1792740/how-to-tell-what-version-of-windows-and-or-cmd-exe-a-batch-file-is-running-on
For /f "tokens=2 delims=[]" %%G in ('ver') Do (set _version=%%G) 
For /f "tokens=2,3,4 delims=. " %%G in ('echo %_version%') Do (set _major=%%G& set _minor=%%H& set _build=%%I) 
if "%_major%"=="5" goto sub5
if "%_major%"=="6" goto sub6
Echo unsupported version >> debugInfo.txt
goto:eof
:sub5
::Winxp or 2003
if "%_minor%"=="2" goto sub_2003
Echo Windows XP [%PROCESSOR_ARCHITECTURE%] >> debugInfo.txt
goto:eof
:sub_2003
Echo Windows 2003 or xp 64 bit [%PROCESSOR_ARCHITECTURE%] >> debugInfo.txt
goto:eof
:sub6
if "%_minor%"=="1" goto sub7
Echo Windows Vista or Windows 2008 [%PROCESSOR_ARCHITECTURE%] >> debugInfo.txt
goto:eof
:sub7
Echo Windows 7 or Windows 2008 R2 [%PROCESSOR_ARCHITECTURE%] >> debugInfo.txt
echo. >> debugInfo.txt

echo current dir: %cd% >> debugInfo.txt
echo. >> debugInfo.txt
dir >> debugInfo.txt


:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: create list of files to zip
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
if NOT exist log\nul goto NOLOG
echo.Extract latest log files for Core and CLR
FOR /F %%I IN ('DIR log\Photon-*.* /B /O:D') DO SET NewestCoreLog=%%I
FOR /F %%I IN ('DIR log\PhotonCLR*.* /B /O:D') DO SET NewestCLRLog=%%I
:NOLOG

echo.Extract latest dump
FOR /F %%I IN ('DIR *.dmp /B /O:D') DO SET NewestDump=%%I

echo.Create debug file list
if exist debugInfoFiles.txt del debugInfoFiles.txt
echo debugInfo.txt >> debugInfoFiles.txt
echo version.txt >> debugInfoFiles.txt
echo PhotonServer.config >> debugInfoFiles.txt
echo log\%NewestCLRLog% >> debugInfoFiles.txt
echo log\%NewestCoreLog% >> debugInfoFiles.txt
echo %NewestDump% >> debugInfoFiles.txt
dir ..\log /b /s /a-d | findstr /vi "Counter.log$" >> debugInfoFiles.txt

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
echo.Create archive
start ..\bin_tools\7zip\7za.exe a -ssw debugInfo_%V_Year%-%V_Month%-%V_Day%_%mytime%.7z @debugInfoFiles.txt

::echo.Clean up
::if exist debugInfo.txt del debugInfo.txt
::if exist debugInfoFiles.txt del debugInfoFiles.txt

::pause
exit
