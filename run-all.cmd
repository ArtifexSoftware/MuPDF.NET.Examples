@echo off
setlocal
cd /d "%~dp0"

rem Run all MuPDF.NET.Examples NN-* projects and report PASS/FAIL vs Expected/.
rem Usage:
rem   run-all.cmd
rem   run-all.cmd --update-expected

set "PSARGS="
if /I "%~1"=="--update-expected" set "PSARGS=-UpdateExpected"
if /I "%~1"=="-UpdateExpected" set "PSARGS=-UpdateExpected"
if /I "%~1"=="/UpdateExpected" set "PSARGS=-UpdateExpected"

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0run-all.ps1" %PSARGS%
exit /b %ERRORLEVEL%
