@echo off

rem cd /d "%ProgramFiles%\Rescue\Drive Snapshot" && (
rem 	start "" "osk.exe"
rem	start "" "snapshot.exe"
rem )

start "" "sgBroker\bin\Debug\sgBroker.exe"
start "" "MorphMQserver\bin\x64\Debug\MorphMQserver.exe"
start "" "dbMQserver\bin\Debug\dbMQserver.exe"
start "" "MorphApp\bin\x64\Debug\MorphApp.exe"
exit /b 0