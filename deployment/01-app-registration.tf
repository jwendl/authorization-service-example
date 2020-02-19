resource "azurerm_resource_group" "app_registration" {
  name     = var.key_vault_resource_group_name
  location = var.key_vault_location
}

resource "random_password" "app_password" {
  length           = 64
  special          = true
  override_special = "_%@"
}

resource "azuread_application" "app_registration" {
  name                       = var.key_vault_app_registration_name
  homepage                   = "https://jwendl.net"
  identifier_uris            = ["api://jwpolicymanager"]
  reply_urls                 = ["https://localhost:7071/"]
  available_to_other_tenants = false
  oauth2_allow_implicit_flow = true
  type                       = "webapp/api"

  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000"

    resource_access {
      id   = "e1fe6dd8-ba31-4d61-89e7-88639da4683d"
      type = "Scope"
    }
  }

  app_role {
    allowed_member_types = [
      "User",
      "Application",
    ]

    description  = "Admins can manage roles and perform all task actions"
    display_name = "Admin"
    is_enabled   = true
    value        = "Admin"
  }
}

resource "azuread_application_password" "app_registration" {
  application_object_id = azuread_application.app_registration.id
  value                 = random_password.app_password.result
  end_date              = timeadd(timestamp(), "8760h")
}
