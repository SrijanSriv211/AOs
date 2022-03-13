@ECHO OFF

rem if exist cfg.set==Echo.
if not exist cfg.set==Echo The system cannot find the file specified.
(
echo Operating System = AOs
echo version = 1.7
echo kernel_version = 1.7
echo Build = 1704
echo Username = %username%

echo.

echo BIOS = 1
echo Boot = 0
echo Log = 1
echo Operation = 1
echo Sysfail = 1
echo Package = 0
echo Launch = 0
echo Late = 1
echo null = 0

echo.

echo 7z.exe
echo 72x.exe
echo Adapter.exe
echo API.exe
echo Bit.exe
echo Root.exe
echo Terminal.exe
) >cfg.set
