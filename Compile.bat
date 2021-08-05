@ECHO OFF

title Compile AOs

echo Compiling.
dotnet publish -c Release -o ./AOs
