output "object_id" {
  value = azuread_application.app_registration.object_id
}

output "client_id" {
  value = azuread_application.app_registration.application_id
}

output "keyvault_secret_uri" {
  value = azurerm_key_vault_secret.key_vault_secret.id
}