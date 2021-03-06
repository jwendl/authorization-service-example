#!/bin/bash

export ARM_SUBSCRIPTION_ID="959965bb-d9df-4c6f-825e-37e1090d817d"
export ARM_CLIENT_ID="6ac75dd9-2472-40c8-bda6-5be40f6324f9"
export ARM_CLIENT_SECRET=""
export ARM_TENANT_ID="72f988bf-86f1-41af-91ab-2d7cd011db47"
export ARM_ACCESS_KEY=""

export TF_VAR_key_vault_resource_group_name="KeyVault"
export TF_VAR_key_vault_location="westus2"
export TF_VAR_key_vault_name="jwapikeyvault"
export TF_VAR_key_vault_tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
export TF_VAR_terraform_tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
export TF_VAR_terraform_object_id="84783e6e-8d71-4f78-867b-104338ed6618"
export TF_VAR_policy_manager_resource_group_name="PolicyManager"
export TF_VAR_policy_manager_location="westus2"
export TF_VAR_policy_manager_storage_account_name="jwpolicymanagerstorage"
export TF_VAR_policy_manager_functions_plan_name="jwpolicymanagerplan"
export TF_VAR_policy_manager_functions_name="jwpolicymanagerapi"
export TF_VAR_app_audience="api://jwpolicymanager"
export TF_VAR_app_tenant_id="1e1a5e3a-f0ea-45c9-9d80-d88e4d8ed378"
export TF_VAR_app_client_id="0b861016-b56c-49b0-899a-b2e24537cb1d"
export TF_VAR_app_client_secret_key_vault_uri="https://jwappkeyvault.vault.azure.net/secrets/app-client-secret/"
export TF_VAR_policy_manager_app_insights_name="jwpolicymanagerai"
export TF_VAR_policy_manager_sql_server_name="jwpolicymanagerdb"
export TF_VAR_policy_manager_sql_database_name="PolicyManager"
export TF_VAR_policy_manager_sql_tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
export TF_VAR_policy_manager_sql_object_id="2609bcf7-475f-49c0-8040-85e667b4f3aa"
export TF_VAR_customer_data_resource_group_name="CustomerData"
export TF_VAR_customer_data_location="westus2"
export TF_VAR_customer_data_storage_account_name="jwcustomerdatastorage"
export TF_VAR_customer_data_functions_plan_name="jwcustomerdataplan"
export TF_VAR_customer_data_functions_name="jwcustomerdataapi"
export TF_VAR_customer_data_app_insights_name="jwcustomerdataai"
export TF_VAR_customer_data_sql_server_name="jwcustomerdatadb"
export TF_VAR_customer_data_sql_database_name="CustomerData"
export TF_VAR_customer_data_sql_tenant_id="72f988bf-86f1-41af-91ab-2d7cd011db47"
export TF_VAR_customer_data_sql_object_id="2609bcf7-475f-49c0-8040-85e667b4f3aa"
export TF_VAR_order_data_resource_group_name="OrderData"
export TF_VAR_order_data_location="westus2"
export TF_VAR_order_data_storage_account_name="jworderdatastorage"
export TF_VAR_order_data_functions_plan_name="jworderdataplan"
export TF_VAR_order_data_functions_name="jworderdataapi"
export TF_VAR_order_data_app_insights_name="jworderdataai"
export TF_VAR_order_data_cosmos_server_name="jworderdatadb"
export TF_VAR_order_data_cosmos_failover_location="eastus"
export TF_VAR_order_data_cosmos_sql_database="OrderData"
export TF_VAR_dashboard_resource_group_name="Dashboard"
export TF_VAR_dashboard_name="jwdashboard"
export TF_VAR_dashboard_location="westus2"

mkdir -p ~/tfoutput
terraform init
terraform plan --out ~/tfoutput/api-example-app
terraform apply ~/tfoutput/api-example-app
