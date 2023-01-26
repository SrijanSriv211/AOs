@ECHO OFF

title Starting build

if EXIST AOs rmdir /s /q AOs
dotnet publish -c Release -o ./AOs

robocopy "Sysfail" "AOs/Sysfail" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
robocopy "Sysfail/RECOVERY" "AOs" /e /nfl /ndl /njh /njs /nc /ns /np /is /it
rmdir /s /q bin,obj
