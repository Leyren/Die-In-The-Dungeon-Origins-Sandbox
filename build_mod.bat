SET OLDDIR=%CD%
dotnet build
cd bin/Debug/netstandard2.1
xcopy DieInTheDungeonSandbox.dll "../../../../BepInEx/plugins" /v /y
cd "C:\Program Files (x86)\Steam"
steam -applaunch 2428770
cd %OLDDIR%