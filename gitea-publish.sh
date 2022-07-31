#!/bin/sh
set -e
# Get current version
VERSION=`date --iso-8601`
cd .build/bin/Release
# Create hashes of all the files (except the hash files themselves & deps.json files)
rm -f ./*.deps.json _sha256sums.txt _sha512sums.txt _blake3sums.txt
SHA256SUMS=`sha256sum *`
SHA512SUMS=`sha512sum *`
BLAKE3SUMS=`b3sum *`
echo "$SHA256SUMS" > _sha256sums.txt
echo "$SHA512SUMS" > _sha512sums.txt
echo "$BLAKE3SUMS" > _blake3sums.txt
# Create args to be passed to tea for uploading all the files
FILES_ARG=`find ./ -type f | awk 1 ORS=' -a '`
FILES_ARG="-a ${FILES_ARG%????}"
# Create a gitea release draft
tea release create --draft --target main \
  --tag v$VERSION --title v$VERSION \
  $FILES_ARG
cd ../..
