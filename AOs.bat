@echo off
set "terminal_exe=%LOCALAPPDATA%\Microsoft\WindowsApps\wt.exe"

if exist "%terminal_exe%" (
    @REM https://stackoverflow.com/a/62832189/18121288
    @REM The following command will be used to launch AOs in windows temrinal
    wt nt cmd /k AOs.exe
) else (
    AOs.exe
)
