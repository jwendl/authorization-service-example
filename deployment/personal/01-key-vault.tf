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

  network_acls {
    default_action = "Deny"
    ip_rules       = ["24.19.163.204"]
    bypass         = "AzureServices"
  }
}

resource "azurerm_key_vault_access_policy" "key_vault_ap1" {
  key_vault_id = azurerm_key_vault.key_vault.id

  tenant_id = var.terraform_tenant_id
  object_id = var.terraform_object_id

  secret_permissions = [
    "backup", "get", "list", "set",
  ]
}
