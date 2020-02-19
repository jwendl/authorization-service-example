resource "azurerm_resource_group" "key_vault" {
  name     = var.key_vault_resource_group_name
  location = var.key_vault_location
}

resource "azurerm_key_vault" "key_vault" {
  name                        = var.key_vault_name
  resource_group_name         = azurerm_resource_group.key_vault.name
  location                    = azurerm_resource_group.key_vault.location
  enabled_for_disk_encryption = true
  tenant_id                   = var.key_vault_tenant_id

  sku_name = "standard"

  access_policy {
    tenant_id = var.key_vault_tenant_id
    object_id = var.key_vault_object_id

    key_permissions = [
      "get",
    ]

    secret_permissions = [
      "get",
    ]

    storage_permissions = [
      "get",
    ]
  }

  network_acls {
    default_action = "Deny"
    bypass         = "AzureServices"
  }
}
