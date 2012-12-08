#!/bin/sh

if [ -z "$HTTP_PROXY" ]; then
  echo "If your are behind a proxy, please configure HTTP_PROXY enviroment variable"
fi
mono --runtime=v4.0.30319 ../src/.nuget/nuget.exe install "../src/GestUAB/packages.config" -source "" -o "../src/packages"
mono --runtime=v4.0.30319 ../src/.nuget/nuget.exe install "../src/GestUAB.Tests/packages.config" -source "" -o "../src/packages"
