provider "azurerm" {
  features {}
}

terraform {
  backend "azurerm" {
    resource_group_name  = "test"
    storage_account_name = "adamtest12345"
    container_name       = "tfstate"
    key                  = "test.terraform.tfstate"
  }
}

# terraform {
#   backend "azurerm" {
#     storage_account_name = "abcd1234"
#     container_name       = "tfstate"
#     key                  = "test.terraform.tfstate"
#     use_msi              = true
#     subscription_id      = "00000000-0000-0000-0000-000000000000"
#     tenant_id            = "00000000-0000-0000-0000-000000000000"
#   }
# }