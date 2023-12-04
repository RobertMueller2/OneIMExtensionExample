@echo off

SETLOCAL ENABLEDELAYEDEXPANSION

:: Check if the configuration file exists
if not exist msbuild.cfg (
    echo Error: msbuild.cfg does not exist. Please copy either of the msbuild.*.example.cfg files to msbuild.cfg.
    pause
    exit /b 1
)

:: Include the configuration file
for /f "delims=" %%x in (msbuild.cfg) do (set "%%x")


echo Available configurations:
echo(

:: Find the line with <PreprocessorVersion> tag
for /f "delims=" %%a in ('findstr /i /c:"<PreprocessorVersion>" "localsettings.props"') do (
  set "line=%%a"

  :: Remove the opening and closing tags
  set "config=!line:<PreprocessorVersion>=!"
  set "config=!config:</PreprocessorVersion>=!"

  echo !config!
)

set /p UserChoice=Enter your desired configuration: 

:: Use the MSBUILD_PATH variable set in the configuration file and the user's choice for configuration
%MSBUILD_PATH% /t:Build /p:Configuration=%UserChoice%

pause