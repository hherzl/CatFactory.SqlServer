cls
set initialPath=%cd%
set srcPath=%cd%\src\CatFactory.SqlServer
set testPath=%cd%\test\CatFactory.SqlServer.Tests
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
