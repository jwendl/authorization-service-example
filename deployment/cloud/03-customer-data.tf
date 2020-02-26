resource "azurerm_resource_group" "customer_data" {
  name     = var.customer_data_resource_group_name
  location = var.customer_data_location
}

resource "random_password" "customer_data_sql_password" {
  length           = 64
  special          = true
  override_special = "_%@"
}

resource "azurerm_storage_account" "customer_data" {
  name                     = var.customer_data_storage_account_name
  resource_group_name      = azurerm_resource_group.customer_data.name
  location                 = azurerm_resource_group.customer_data.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "customer_data" {
  name                = var.customer_data_functions_plan_name
  location            = azurerm_resource_group.customer_data.location
  resource_group_name = azurerm_resource_group.customer_data.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "customer_data" {
  name                      = var.customer_data_functions_name
  location                  = azurerm_resource_group.customer_data.location
  resource_group_name       = azurerm_resource_group.customer_data.name
  app_service_plan_id       = azurerm_app_service_plan.customer_data.id
  storage_connection_string = azurerm_storage_account.customer_data.primary_connection_string
  version                   = "~3"
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.customer_data.instrumentation_key
    FUNCTIONS_WORKER_RUNTIME       = "dotnet"
    WEBSITE_RUN_FROM_PACKAGE       = "1"
    TokenValidator__Audience       = var.app_audience
    TokenValidator__TenantId       = var.app_tenant_id
    TokenValidator__ClientId       = var.app_client_id
    Database__ConnectionString     = "'Server=tcp:${azurerm_sql_server.customer_data.fully_qualified_domain_name},1433;Database=${azurerm_sql_database.customer_data.name};"
  }

  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}

resource "azurerm_application_insights" "customer_data" {
  name                = var.customer_data_app_insights_name
  location            = azurerm_resource_group.customer_data.location
  resource_group_name = azurerm_resource_group.customer_data.name
  application_type    = "web"
}

resource "azurerm_sql_server" "customer_data" {
  name                         = var.customer_data_sql_server_name
  resource_group_name          = azurerm_resource_group.customer_data.name
  location                     = azurerm_resource_group.customer_data.location
  version                      = "12.0"
  administrator_login          = "jwendl"
  administrator_login_password = random_password.customer_data_sql_password.result
}

resource "azurerm_sql_database" "customer_data" {
  name                = var.customer_data_sql_database_name
  resource_group_name = azurerm_resource_group.customer_data.name
  location            = azurerm_resource_group.customer_data.location
  server_name         = azurerm_sql_server.customer_data.name
}

resource "azurerm_sql_active_directory_administrator" "customer_data" {
  server_name         = azurerm_sql_server.customer_data.name
  resource_group_name = azurerm_resource_group.customer_data.name
  login               = "sqladmin"
  tenant_id           = var.customer_data_sql_tenant_id
  object_id           = var.customer_data_sql_object_id
}

output "customer_data_instrumentation_key" {
  value = "${azurerm_application_insights.customer_data.instrumentation_key}"
}

output "customer_data_app_id" {
  value = "${azurerm_application_insights.customer_data.app_id}"
}