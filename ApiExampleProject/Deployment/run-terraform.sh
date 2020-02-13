#!/bin/bash

export ARM_SUBSCRIPTION_ID="959965bb-d9df-4c6f-825e-37e1090d817d"
export ARM_CLIENT_ID="6ac75dd9-2472-40c8-bda6-5be40f6324f9"
export ARM_CLIENT_SECRET=""
export ARM_TENANT_ID="72f988bf-86f1-41af-91ab-2d7cd011db47"

mkdir -p ~/tfoutput
terraform plan \
    -var "dashboard_resource_group_name=Dashboard" \
    -var "dashboard_name=jwdashboard" \
    -var "dashboard_location=westus2" \
    -var "key_vault_app_registration_name=jwpolicymanager" \
    -var "key_vault_resource_group_name=KeyVault" \
    -var "key_vault_location=westus2" \
    -var "key_vault_name=jwapikeyvault" \
    -var "key_vault_tenant_id=72f988bf-86f1-41af-91ab-2d7cd011db47" \
    -var "key_vault_object_id=8f6370ec-e73b-4dd1-b7de-f7b1cd1644b9" \
    -var "policy_manager_resource_group_name=PolicyManager" \
    -var "policy_manager_location=westus2" \
    -var "policy_manager_storage_account_name=jwpolicymanagerstorage" \
    -var "policy_manager_functions_plan_name=jwpolicymanagerplan" \
    -var "policy_manager_functions_name=jwpolicymanagerapi" \
    -var "policy_manager_app_insights_name=jwpolicymanagerai" \
    -var "policy_manager_sql_server_name=jwpolicymanagerdb" \
    -var "policy_manager_sql_admin_login=jwendl" \
    -var "policy_manager_sql_admin_password=Ihm5pwd!" \
    -var "policy_manager_sql_database_name=PolicyManager" \
    -var "policy_manager_sql_tenant_id=72f988bf-86f1-41af-91ab-2d7cd011db47" \
    -var "policy_manager_sql_object_id=2609bcf7-475f-49c0-8040-85e667b4f3aa" \
    --out ~/tfoutput/api-example-app
terraform apply ~/tfoutput/api-example-app
