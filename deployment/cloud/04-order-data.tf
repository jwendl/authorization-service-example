resource "azurerm_resource_group" "order_data" {
  name     = var.order_data_resource_group_name
  location = var.order_data_location
}

resource "azurerm_storage_account" "order_data" {
  name                     = var.order_data_storage_account_name
  resource_group_name      = azurerm_resource_group.order_data.name
  location                 = azurerm_resource_group.order_data.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "order_data" {
  name                = var.order_data_functions_plan_name
  location            = azurerm_resource_group.order_data.location
  resource_group_name = azurerm_resource_group.order_data.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "order_data" {
  name                      = var.order_data_functions_name
  location                  = azurerm_resource_group.order_data.location
  resource_group_name       = azurerm_resource_group.order_data.name
  app_service_plan_id       = azurerm_app_service_plan.order_data.id
  storage_connection_string = azurerm_storage_account.order_data.primary_connection_string
  version                   = "~3"
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY        = azurerm_application_insights.order_data.instrumentation_key
    FUNCTIONS_WORKER_RUNTIME              = "dotnet"
    WEBSITE_RUN_FROM_PACKAGE              = "1"
    TokenValidator__Audience              = var.app_audience
    TokenValidator__TenantId              = var.app_tenant_id
    TokenValidator__ClientId              = var.app_client_id
    CosmosConfiguration__EndpointLocation = azurerm_cosmosdb_account.order_data.endpoint
    CosmosConfiguration__PrimaryKey       = format("@Microsoft.KeyVault(SecretUri=%s)", azurerm_key_vault_secret.key_vault_secret.id)
  }

  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}

resource "azurerm_application_insights" "order_data" {
  name                = var.order_data_app_insights_name
  location            = azurerm_resource_group.order_data.location
  resource_group_name = azurerm_resource_group.order_data.name
  application_type    = "web"
}

resource "azurerm_cosmosdb_account" "order_data" {
  name                = var.order_data_cosmos_server_name
  location            = azurerm_resource_group.order_data.location
  resource_group_name = azurerm_resource_group.order_data.name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = true

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }

  geo_location {
    location          = var.order_data_cosmos_failover_location
    failover_priority = 1
  }

  geo_location {
    prefix            = "${var.order_data_cosmos_server_name}-failover"
    location          = azurerm_resource_group.order_data.location
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_sql_database" "order_data" {
  name                = var.order_data_cosmos_sql_database
  resource_group_name = azurerm_cosmosdb_account.order_data.resource_group_name
  account_name        = azurerm_cosmosdb_account.order_data.name
  throughput          = 400
}

resource "azurerm_key_vault_secret" "key_vault_secret" {
  name         = "cosmos-primary-key"
  value        = azurerm_cosmosdb_account.order_data.primary_master_key
  key_vault_id = azurerm_key_vault.key_vault.id
}

output "order_data_instrumentation_key" {
  value = "${azurerm_application_insights.order_data.instrumentation_key}"
}
