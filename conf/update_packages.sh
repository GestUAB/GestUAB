#!/bin/sh

if [ -z "$HTTP_PROXY" ]; then
  echo "If your are behind a proxy, please configure HTTP_PROXY enviroment variable"
fi
mono --runtime=v4.0.30319 ../.nuget/nuget.exe install "../src/packages.config" -source "" -o "../packages"