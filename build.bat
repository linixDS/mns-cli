@echo off

if "%1" == "install" (
     if not exist ".\build" mkdir .\build

     if not exist ".\build\modules" mkdir .\build\modules


     if not exist ".\build\modules\fortigate" mkdir .\build\modules\fortigate
     copy .\fortigate\bin\Debug\net7.0\fortigate.dll .\.\build\modules\fortigate\
     copy .\fortigate\bin\Debug\net7.0\core.dll .\.\build\modules\fortigate\

     if not exist ".\build\modules\network" mkdir .\build\modules\network

     copy .\network\bin\Debug\net7.0\network.dll .\.\build\modules\network\
     copy .\network\bin\Debug\net7.0\pacgae.dll .\.\build\modules\network\
     copy .\network\bin\Debug\net7.0\core.dll .\.\build\modules\network\

     copy .\mns-cli\bin\Debug\net7.0\mns-cli.dll .\.\build\
     copy .\mns-cli\bin\Debug\net7.0\mns-cli.exe .\.\build\
     copy .\core\bin\Debug\net7.0\core.dll .\.\build\
     copy .\runtime\bin\Debug\net7.0\runtime.dll .\.\build\

    exit 0
)

if "%1" == "compile" (
    echo "----------------------------------------------"
    echo "::Compiling library LibTerminal.dll"
    echo "----------------------------------------------"
    cd .\core\
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "::Compiling program mns-cli"
    echo "----------------------------------------------"
    cd .\mns-cli\
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "::Compiling module network.dll"
    echo "----------------------------------------------"
    cd .\network\
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "Compiling module fortigate.dll"
    echo "----------------------------------------------"
    cd .\fortigate\
    dotnet build
    cd ..
    exit 0
)

echo "build.bat compile"
echo "build.bat install"



