#!/bin/sh
APP_PATH=$1
APP_NAME=$2
SDK_DIR=$3
COMPANY_NAME=
BUILD_PATH=$1"/build/"$2

cd $BUILD_PATH
export PATH=$3/tools:$3/platform-tools:$PATH
android update project -p . -n $2
ant clean
ant debug
cp bin/$2-debug.apk $APP_PATH/

cd $APP_PATH
sh $APP_PATH/Assets/Editor/PostprocessBuildPlayerOrig $BUILD_PATH/bin/$APP_NAME-debug.apk android "" $COMPANY_NAME $APP_NAME
