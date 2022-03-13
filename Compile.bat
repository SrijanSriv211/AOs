@ECHO OFF

title Compile AOs
echo Compiling AOs.

if EXIST AOs rmdir /s /q AOs
dotnet publish -c Release -o ./AOs

robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
rmdir /s /q bin,obj

if EXIST AOs\AOs.exe echo AOs Compiled Successfully!
if NOT EXIST AOs\AOs.exe echo Can't Compile AOs!
