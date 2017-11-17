USER="thorium"

su $USER <<EOM
cd $PWD
git fetch
git pull
git submodule update --init --recursive
git submodule update --recursive --remote
nuget restore
msbuild
EOM