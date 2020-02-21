variable "dashboard_resource_group_name" {
  description = "The dashboard resource group"
}

variable "dashboard_name" {
  description = "The dashboard name"
}

variable "dashboard_location" {
  description = "The dashboard location"
}

variable "app_client_secret_key_vault_uri" {
  description = "The reference to the client app secret"
}

variable "policy_manager_resource_group_name" {
  description = "The policy manager resource group"
}

variable "policy_manager_location" {
  description = "The policy manager location"
}

variable "policy_manager_storage_account_name" {
  description = "The policy manager storage account name"
}

variable "policy_manager_functions_plan_name" {
  description = "The policy manager service app plan name"
}

variable "policy_manager_functions_name" {
  description = "The policy manager functions app name"
}

variable "policy_manager_app_insights_name" {
  description = "The policy manager app insights name"
}

variable "policy_manager_sql_server_name" {
  description = "The policy manager sql server name"
}

variable "policy_manager_sql_database_name" {
  description = "The policy manager sql admin password"
}

variable "policy_manager_sql_tenant_id" {
  description = "The tenant id of the admin for sql server"
}

variable "policy_manager_sql_object_id" {
  description = "The object id of the admin for sql server"
}