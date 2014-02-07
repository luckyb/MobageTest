#!/bin/sh
cd $1
export PATH=$3/tools:$3/platform-tools:$PATH
android update project -p . -n $2
ant clean
ant debug
adb install -r bin/$2-debug.apk
