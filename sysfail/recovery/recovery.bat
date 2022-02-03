@echo off
cls
rem mode con: lines=42 cols=119

:1
echo.
echo Welcome to AOs 1.3 OS Recovery
echo Recovery AOs 1.3
echo.
goto recovery

:recovery
echo Recovering system...
echo.

echo Recovering 7z.exe...
xcopy sysfail\recovery\7z.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering 72x.exe...
xcopy sysfail\recovery\72x.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering Adapter.exe...
xcopy sysfail\recovery\Adapter.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering adm.exe...
xcopy sysfail\recovery\adm.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering API.exe...
xcopy sysfail\recovery\API.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering Bit.exe...
xcopy sysfail\recovery\Bit.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering Root.exe...
xcopy sysfail\recovery\Root.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering Terminal.exe...
xcopy sysfail\recovery\Terminal.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering safe.exe...
xcopy sysfail\recovery\safe.exe /f
ping localhost -n 2,1 >nul
echo.

echo Recovering completed
color 07
exit
