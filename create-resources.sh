#!/bin/bash
echo "Configuring AWS"

aws \
  s3api create-bucket \
  --bucket zenthrill-files \
  --endpoint-url=http://localhost:4566

aws s3api put-bucket-cors --bucket zenthrill-files --endpoint-url=http://localhost:4566 --cors-configuration '{
    "CORSRules": [
        {
            "AllowedOrigins": ["http://localhost:3000"],
            "AllowedMethods": ["GET", "PUT", "POST", "DELETE", "HEAD"],
            "AllowedHeaders": ["*"],
            "MaxAgeSeconds": 3000
        }
    ]
}'

echo "AWS services have been successfully created."