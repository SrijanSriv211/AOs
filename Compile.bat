@ECHO OFF

title Compile AOs

echo Compiling.
dotnet publish -c Release -r win-x64 -o ./AOs /p:PublishSingleFile=true
