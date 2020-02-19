#!/bin/bash

if [ $# -lt 1 ]
then
  echo "Usage: $0 -r <resourceGroup> -n <storageName> -l <location>"
  echo "-r resource group name"
  echo "-n storage name"
  echo "-l location"
  exit 1
fi

resourceGroupName=""
name=""
location=""

while getopts "r:n:l:" arg; do
    case $arg in
        r) resourceGroupName=$OPTARG;;
        n) name=$OPTARG;;
        l) location=$OPTARG;;
    esac
done

echo "ResourceGroupName = $resourceGroupName"
echo "Storage Name = $name"
echo "Location = $location"

if [ $(az group exists --name $resourceGroupName) = false ]; then
    az group create --name $resourceGroupName --location $location
    az storage account create --name $name --resource-group $resourceGroupName --location $location --sku Standard_LRS --kind StorageV2

    storageKey=$(az storage account keys list --resource-group $resourceGroupName --account-name $name --query "[0].value" --output tsv)
    az storage container create --name terraform-state --account-name $name --account-key $storageKey
fi
