#!/bin/bash

export ARM_SUBSCRIPTION_ID="27339114-1084-4083-91c5-eecdd911de26"
export ARM_CLIENT_ID="62c5da92-df8b-4c84-96d4-2e2ad5a30717"
export ARM_CLIENT_SECRET=""
export ARM_TENANT_ID="1e1a5e3a-f0ea-45c9-9d80-d88e4d8ed378"
export ARM_ACCESS_KEY=""

export TF_VAR_key_vault_app_registration_name="jwpolicymanager"
export TF_VAR_key_vault_resource_group_name="KeyVault"
export TF_VAR_key_vault_location="westus2"
export TF_VAR_key_vault_name="jwappkeyvault"
export TF_VAR_key_vault_tenant_id="1e1a5e3a-f0ea-45c9-9d80-d88e4d8ed378"
export TF_VAR_terraform_tenant_id="1e1a5e3a-f0ea-45c9-9d80-d88e4d8ed378"
export TF_VAR_terraform_object_id="477dc7f8-26f8-4683-b1db-6569507deb35"
export TF_VAR_app_tenant_id="1e1a5e3a-f0ea-45c9-9d80-d88e4d8ed378"
export TF_VAR_app_object_id="8f6370ec-e73b-4dd1-b7de-f7b1cd1644b9"
export TF_VAR_app_registration_name="PolicyManager"

mkdir -p ~/tfoutput
terraform init
terraform plan --out ~/tfoutput/api-example-app
terraform apply ~/tfoutput/api-example-app
