provider "azurerm" {
  version = "~> 1.41"
}

provider "azuread" {
  version = "~> 0.7"
}

provider "random" {
  version = "~> 2.2"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "TerraformBackend"
    storage_account_name = "jwtfbackend"
    container_name       = "terraform-personal-state"
    key                  = "prod.terraform.tfstate"
  }
}