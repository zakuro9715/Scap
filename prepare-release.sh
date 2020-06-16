mkdir -p ./dist/EasyCapture
cp -R EasyCapture.Settings/bin/Release/netcoreapp3.1/* ./dist/EasyCapture/
cp -R EasyCapture/bin/Release/netcoreapp3.1/* ./dist/EasyCapture/

cd dist
zip -r EasyCapture.zip EasyCapture
