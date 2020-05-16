@ECHO OFF

PUSHD NSonic.Tests

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
REM Previous line will generate coverage.cobertura.xml
tools\reportgenerator.exe -reports:coverage.cobertura.xml -targetdir:coverage -reporttypes:Html
explorer coverage\index.htm

POPD
