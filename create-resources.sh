#!/bin/bash
echo "Configuring AWS"

aws \
  s3api create-bucket \
  --bucket zenthrill-files \
  --endpoint-url=http://localhost:4566

echo "AWS services have been successfully created."