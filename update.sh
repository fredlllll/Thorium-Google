USER="thorium"

su $USER <<EOM
git pull
git submodule update --init --recursive
git submodule update --recursive --remote
cd Thorium-Computing-Farm
git checkout master
git pull
cd Source
nuget restore
cd ../..
nuget restore
msbuild
EOM