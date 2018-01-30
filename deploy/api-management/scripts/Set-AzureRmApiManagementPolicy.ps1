Param(
    [string] $apiManagementInstanceName = $(throw "The API Management instance name is required"),
    [string] $resourceGroupName = $(throw "Resource group name is required"),
    [string] $policyDefinitionPath = $(throw "Path to the policy definition is required"),
    [string]$productId = $(throw "Id of the product to apply the policy to is required"),
    [bool]$isLocalExecution = $false,
    [string] $subscriptionId = ""
)

function Autenticate ([string]$subscriptionId) {
    Login-AzureRmAccount
    Set-AzureRmContext -SubscriptionId $subscriptionId
}

function Verify-PolicyDefinitionLocation ([string]$policyDefinitionPath) {
    if(!(Test-Path $policyDefinitionPath)) {
        Throw [System.IO.FileNotFoundException] "A valid policy file was not found at $swaggerDefinitionPath"
    }

    Write-Host "Policy definition exists on the following location: $policyDefinitionPath"
}

function Set-AzureApiManagementPolicyForProduct ([string]$apiManagementInstanceName,[string]$resourceGroupName, [string]$policyDefinitionPath, [string]$productId) {
    $policyDefinition = Get-Content $policyDefinitionPath
    Write-Host "Applying policy: $policyDefinition" -Verbose

    $apiManagementContext = New-AzureRmApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apiManagementInstanceName
    Set-AzureRmApiManagementPolicy -Context $apiManagementContext -PolicyFilePath $policyDefinitionPath -ProductId $productId -Verbose
    Write-Host "Policy applied to product $productId for instance $apiManagementInstanceName"
}

# Allow authentication when running locally
if($isLocalExecution -eq $true){
    Authenticate -subscriptionId $subscriptionId
}

# Verify that the policy definition exists
Verify-PolicyDefinitionLocation -policyDefinitionPath $policyDefinitionPath

# Apply the policy to the product
Set-AzureApiManagementPolicyForProduct -apiManagementInstanceName $apiManagementInstanceName -resourceGroupName $resourceGroupName -policyDefinitionPath $policyDefinitionPath -productId $productId