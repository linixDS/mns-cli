@echo off

if "%1" == "install" (
     if not exist ".\build" mkdir .\build

     if not exist ".\build\modules" mkdir .\build\modules


     if not exist ".\build\modules\fortigate" mkdir .\build\modules\fortigate
     copy .\fortigate\bin\Debug\net7.0\fortigate.dll .\.\build\modules\fortigate\
     copy .\fortigate\bin\Debug\net7.0\LibTerminal.dll .\.\build\modules\fortigate\

     if not exist ".\build\modules\network" mkdir .\build\modules\network

     cp .\network\bin\Debug\net7.0\network.dll .\.\build\modules\network\
     cp .\network\bin\Debug\net7.0\LibTerminal.dll .\.\build\modules\network\

    exit 0
)

if "%1" == "compile" (
    echo "----------------------------------------------"
    echo "::Compiling library LibTerminal.dll"
    echo "----------------------------------------------"
    cd ./LibTerminal/
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "::Compiling program mns-cli"
    echo "----------------------------------------------"
    cd ./mns-cli/
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "::Compiling module network.dll"
    echo "----------------------------------------------"
    cd ./network/
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "Compiling module fortigate.dll"
    echo "----------------------------------------------"
    cd ./fortigate/
    dotnet build
    cd ..
    exit 0
)

echo "build.bat compile"
echo "build.bat install"



