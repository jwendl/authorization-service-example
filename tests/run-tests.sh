#!/bin/bash

dotnet publish ./ApiExampleProject.IntegrationTests.sln --output ./output/

cd output
npm i azure-functions-core-tools@3 --unsafe-perm true
chmod +x ./node_modules/azure-functions-core-tools/bin/func

dotnet vstest ApiExampleProject.IntegrationTests.dll --logger:"trx;LogFileName=integration-results.trx"
