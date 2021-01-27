@ECHO OFF

title Compile AOs

echo Compiling.
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true

move .\bin\Release\netcoreapp3.1\win-x64\publish\AOs.exe .\AOs\AOs.exe
exit
