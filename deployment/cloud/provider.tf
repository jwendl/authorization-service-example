provider "azurerm" {
  version = "~> 2.0"
  features {
    virtual_machine {
      delete_os_disk_on_deletion = true
    }
    virtual_machine_scale_set {
      roll_instances_when_required = true
    }
    key_vault {
      recover_soft_deleted_key_vaults = true
      purge_soft_delete_on_destroy = true
    }
  }
}

provider "random" {
  version = "~> 2.2"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "TerraformBackend"
    storage_account_name = "jwtfbackend"
    container_name       = "terraform-cloud-state"
    key                  = "prod.terraform.tfstate"
  }
}