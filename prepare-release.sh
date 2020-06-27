mkdir -p ./dist/Scap
cp -R Scap.Settings/bin/Release/netcoreapp3.1/* ./dist/Scap/
cp -R Scap/bin/Release/netcoreapp3.1/* ./dist/Scap/

cd dist
zip -r Scap.zip Scap
