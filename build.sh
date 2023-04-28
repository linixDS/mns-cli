#!/bin/bash

if [ "$1" == "install" ] ; then
    if [ ! -d "./build" ]; then
         mkdir ./build
    fi

    if [ ! -d "./build/modules" ]; then
         mkdir ./build/modules
    fi

    if [ ! -d "./build/modules/fortigate" ]; then
         mkdir ./build/modules/fortigate
    fi

    cp ./fortigate/bin/Debug/net7.0/fortigate.dll ././build/modules/fortigate/
    cp ./fortigate/bin/Debug/net7.0/core.dll ././build/modules/fortigate/
    cp ./fortigate/bin/Debug/net7.0/package.json ././build/modules/fortigate/

    if [ ! -d "./build/modules/module" ]; then
         mkdir ./build/modules/module
    fi

    cp ./module/bin/Debug/net7.0/module.dll ././build/modules/module/
    cp ./module/bin/Debug/net7.0/core.dll ././build/modules/module/
    cp ./module/bin/Debug/net7.0/package.json ././build/modules/module/



    if [ ! -d "./build/modules/network" ]; then
         mkdir ./build/modules/network
    fi

    cp ./network/bin/Debug/net7.0/network.dll ././build/modules/network/
    cp ./network/bin/Debug/net7.0/core.dll ././build/modules/network/
    cp ./network/bin/Debug/net7.0/package.json ././build/modules/network/


    if [ ! -d "./build/modules/remote" ]; then
         mkdir ./build/modules/remote
    fi

    cp ./remote/bin/Debug/net7.0/remote.dll ././build/modules/remote/
    cp ./remote/bin/Debug/net7.0/core.dll ././build/modules/remote/
    cp ./remote/bin/Debug/net7.0/package.json ././build/modules/remote/

    cp ./mns-cli/bin/Debug/net7.0/mns-cli.dll ././build/
    cp ./mns-cli/bin/Debug/net7.0/mns-cli ././build/
    cp ./core/bin/Debug/net7.0/core.dll ././build/
    cp ./runtime/bin/Debug/net7.0/runtime.dll ././build/

    cp ./mns-cli/bin/Debug/net7.0/mns-cli.runtimeconfig.json ././build/

    exit 0
fi

if [ "$1" == "compile" ] ; then
    echo "----------------------------------------------"
    echo "::Compiling library core.dll"
    echo "----------------------------------------------"
    cd ./core/
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "::Compiling library runtime.dll"
    echo "----------------------------------------------"
    cd ./runtime/
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

    echo "----------------------------------------------"
    echo "Compiling module remote.dll"
    echo "----------------------------------------------"
    cd ./remote/
    dotnet build
    cd ..

    echo "----------------------------------------------"
    echo "Compiling module module.dll"
    echo "----------------------------------------------"
    cd ./module/
    dotnet build
    cd ..

    exit 0
fi

echo "build.sh compile"
echo "build.sh install"



