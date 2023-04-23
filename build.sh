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
    cp ./fortigate/bin/Debug/net7.0/LibTerminal.dll ././build/modules/fortigate/

    if [ ! -d "./build/modules/network" ]; then
         mkdir ./build/modules/network
    fi
    cp ./network/bin/Debug/net7.0/network.dll ././build/modules/network/
    cp ./network/bin/Debug/net7.0/LibTerminal.dll ././build/modules/network/

    exit 0
fi

if [ "$1" == "compile" ] ; then

    cd ./LibTerminal/
    dotnet build
    cd ..

    cd ./mns-cli/
    dotnet build
    cd ..

    cd ./geoip/
    dotnet build
    cd ..

    cd ./network/
    dotnet build
    cd ..

    exit 0
fi

echo "build.sh compile"
echo "build.sh install"



