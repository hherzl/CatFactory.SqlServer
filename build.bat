cls
set initialPath=%cd%
set srcPath=%cd%\CatFactory.SqlServer
set testPath=%cd%\CatFactory.SqlServer.Tests
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
