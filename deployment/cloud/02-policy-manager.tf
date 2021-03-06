resource "azurerm_resource_group" "policy_manager" {
  name     = var.policy_manager_resource_group_name
  location = var.policy_manager_location
}

resource "random_password" "policy_manager_sql_password" {
  length           = 64
  special          = true
  override_special = "_%@"
}

resource "azurerm_storage_account" "policy_manager" {
  name                     = var.policy_manager_storage_account_name
  resource_group_name      = azurerm_resource_group.policy_manager.name
  location                 = azurerm_resource_group.policy_manager.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "policy_manager" {
  name                = var.policy_manager_functions_plan_name
  location            = azurerm_resource_group.policy_manager.location
  resource_group_name = azurerm_resource_group.policy_manager.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "policy_manager" {
  name                      = var.policy_manager_functions_name
  location                  = azurerm_resource_group.policy_manager.location
  resource_group_name       = azurerm_resource_group.policy_manager.name
  app_service_plan_id       = azurerm_app_service_plan.policy_manager.id
  storage_connection_string = azurerm_storage_account.policy_manager.primary_connection_string
  version                   = "~3"
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.policy_manager.instrumentation_key
    FUNCTIONS_WORKER_RUNTIME       = "dotnet"
    WEBSITE_RUN_FROM_PACKAGE       = "1"
    TokenValidator__Audience       = var.app_audience
    TokenValidator__TenantId       = var.app_tenant_id
    TokenValidator__ClientId       = var.app_client_id
    TokenCreator__TenantId         = var.app_tenant_id
    TokenCreator__ClientId         = var.app_client_id
    TokenCreator__ClientSecret     = format("@Microsoft.KeyVault(SecretUri=%s)", var.app_client_secret_key_vault_uri)
    TokenCreator__Scopes           = "https://graph.microsoft.com/User.Read"
    Database__ConnectionString     = "'Server=tcp:${azurerm_sql_server.policy_manager.fully_qualified_domain_name},1433;Database=${azurerm_sql_database.policy_manager.name};"
  }

  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}

resource "azurerm_application_insights" "policy_manager" {
  name                = var.policy_manager_app_insights_name
  location            = azurerm_resource_group.policy_manager.location
  resource_group_name = azurerm_resource_group.policy_manager.name
  application_type    = "web"
}

resource "azurerm_sql_server" "policy_manager" {
  name                         = var.policy_manager_sql_server_name
  resource_group_name          = azurerm_resource_group.policy_manager.name
  location                     = azurerm_resource_group.policy_manager.location
  version                      = "12.0"
  administrator_login          = "jwendl"
  administrator_login_password = random_password.policy_manager_sql_password.result
}

resource "azurerm_sql_database" "policy_manager" {
  name                = var.policy_manager_sql_database_name
  resource_group_name = azurerm_resource_group.policy_manager.name
  location            = azurerm_resource_group.policy_manager.location
  server_name         = azurerm_sql_server.policy_manager.name
}

resource "azurerm_sql_active_directory_administrator" "policy_manager" {
  server_name         = azurerm_sql_server.policy_manager.name
  resource_group_name = azurerm_resource_group.policy_manager.name
  login               = "sqladmin"
  tenant_id           = var.policy_manager_sql_tenant_id
  object_id           = var.policy_manager_sql_object_id
}

output "policy_manager_instrumentation_key" {
  value = "${azurerm_application_insights.policy_manager.instrumentation_key}"
}

output "policy_manager_app_id" {
  value = "${azurerm_application_insights.policy_manager.app_id}"
}