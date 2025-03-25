@echo off

set msbuildcfg=msbuild.cfg
set MSBUILD_PATH=
set havedotnet=
set havemsbuild=

set localsettings=localsettings.props
set configs=configurations.txt
set solution=OneIMExtensionExample.sln

if exist %msbuildcfg% (
    for /f "usebackq delims=" %%x in (%msbuildcfg%) do (set "%%x")
)

if defined MSBUILD_PATH (
  if exist %MSBUILD_PATH% (
    set havemsbuild=12
    set anyfound=1
  )
)

where /Q dotnet.exe && (
  set havedotnet=34
  set anyfound=1
)

if not "%anyfound%"=="1" (
    echo Neither MSBuild nor dotnet build found.
    echo=
    pause
    exit /b 1
)

if not exist %localsettings% (
    echo Error: %localsettings% does not exist. Please copy %localsettings%.template to %localsettings% and edit.
    pause
    exit /b 1
)

for /f "usebackq delims=" %%x in (%msbuildcfg%) do (set "%%x")

set generateconfigs=0
set seen=0

for /F "delims=" %%x in ('dir /B /O:-D /T:W %configs% %solution%') do (
  setlocal enabledelayedexpansion

  if !seen!==0 (
    if %%x==%solution% set generateconfigs=1
    set seen=1
  )
)

if %generateconfigs%==1 (
  if exist %configs% (
    del %configs%
    echo regenerating %configs%
  ) else (
    echo File %configs% does not exist, generating
  )

  setlocal enabledelayedexpansion
  set in_section=0
  for /f "usebackq delims=" %%l in (%solution%) do (
    set "line=%%l"
    set do_output=0

    if !in_section!==2 set in_section=1

    if !in_section!==0 (
      echo "!line!" | findstr /C:"GlobalSection(SolutionConfigurationPlatforms) = preSolution" > nul
      if !errorlevel!==0 set in_section=2
    ) else (
      echo "!line!" | findstr /c:"EndGlobalSection" > nul
      if !errorlevel!==0 set in_section=0
    )

    if !in_section!==1 (
      echo "!line!" | findstr /C:" = " > nul
      if !errorlevel!==0 set do_output=1
    )

    if !do_output!==1 (
      for /f "tokens=*" %%t in ("!line!") do (for /f "tokens=1 delims=|" %%x in ("%%t") do (echo %%x>>"%configs%"))
    )
  )
  endlocal
)

set index=0
echo Available configurations:
echo ---
echo=

for /f "usebackq tokens=*" %%a in (%configs%) do (

  setlocal enabledelayedexpansion

  findstr /C:"'%%a'" %localsettings% >nul
  if !errorlevel!==0 (
    set /a index+=1
    echo !index! - %%a
    set CONFIG_!index!=%%a
  )
)

echo=

set /p choice=Enter your desired configuration: 
set CONFIG=!CONFIG_%choice%!

findstr /R /C:"^%CONFIG%$" %configs% >nul
if %errorlevel% neq 0 (
  echo Configuration "%CONFIG%" not found, aborting.
  pause
  exit /b 1
)

REM einfach öfter mal GOTO wagen ;)
if defined havemsbuild (
  if not defined havedotnet goto build_msbuild
)

if defined havedotnet  (
  if not defined havemsbuild goto build_dotnet
)

echo 1. dotnet
echo 2. MSBuild
choice /c 12 /m "Select build option: "

if errorlevel 2 (
  goto build_msbuild
)
if errorlevel 1 (
  goto build_dotnet
)

:build_msbuild
%MSBUILD_PATH% /p:Configuration=%CONFIG%
goto end

:build_dotnet
dotnet build -c %CONFIG%
goto end

:end
pause