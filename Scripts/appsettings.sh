sed -i 's/win10-x64-v{version}.zip/$1-v{version}.tar.gz/g' EmbyStat.Web/appsettings.json
sed -i 's/0.0.0.0/$(GitVersion.SemVer)/g' EmbyStat.Web/appsettings.json
sed -i 's/RollbarENV/dev/g' EmbyStat.Web/appsettings.json
sed -i 's/XXXXXXXXXX/$2/g' EmbyStat.Web/appsettings.json