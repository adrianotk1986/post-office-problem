set config=%2
set runtime=%1
if "x"=="x%config%" set config=release
if "x"=="x%runtime%" set runtime=win10-x64
dotnet publish src/PostOffice/PostOffice/PostOffice.csproj --framework netcoreapp3.1 --runtime %runtime% --configuration %config%