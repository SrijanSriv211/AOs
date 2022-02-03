@ECHO OFF
:1
cls
color 17
title Error 0x007 - Terminal.exe not found
rem mode con: lines=42 cols=119
echo The %date% at %time% AOs 1.3 crashed because the error 0x007 is occuring. >>crashreport.txt

:2
echo.
echo.
echo Crash Report
echo.
echo Error 0x007: AOs 1.3 can't found external command Terminal.exe.
echo Do you want start recovery ?
set /P us=Continue.
call sysfail/recovery/recovery.bat
