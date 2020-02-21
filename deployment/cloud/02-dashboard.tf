resource "azurerm_resource_group" "dashboard" {
  name     = var.dashboard_resource_group_name
  location = var.dashboard_location
}

resource "azurerm_dashboard" "dashboard" {
  name                 = var.dashboard_name
  resource_group_name  = azurerm_resource_group.dashboard.name
  location             = var.dashboard_location
  dashboard_properties = templatefile("dashboard.tpl", {})
}

