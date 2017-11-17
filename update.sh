USER="thorium"

su $USER <<EOM
git fetch
git pull
git submodule update --init --recursive
git submodule update --recursive --remote
cd Thorium-Computing-Farm/Source
nuget restore
cd ../..
nuget restore
msbuild
EOM