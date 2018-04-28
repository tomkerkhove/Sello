param
(
    [string] $apiManagementInstanceName = $(throw "The API Management instance name is required"),
    [string] $resourceGroupName = $(throw "Resource group name is required"),
    [string] $swaggerDefinitionPath = $(throw "Path to Swagger definition is required"),
    [string] $apiId = $(throw "Id of the logical Api is required"),
    [string] $apiUrlSuffix = $(throw "Url suffix of the api url suffix is required"),
    [string] $apiDefaultName="",
    [string] $apiName="",
    [string] $apiDefaultUrl="localhost:1337",
    [string] $apiUrl = $null,
    [bool]$isLocalExecution = $false,
    [string] $subscriptionId = ""
)

function Authenticate ([string]$subscriptionId) {
    Login-AzureRmAccount
    Set-AzureRmContext -SubscriptionId $subscriptionId
}

function Verify-SwaggerDefinitionLocation ([string]$swaggerDefinitionPath) {
    if(!(Test-Path $swaggerDefinitionPath)) {
        Throw [System.IO.FileNotFoundException] "A valid swagger file was not found at $swaggerDefinitionPath"
    }

    Write-Host "Swagger definition exists on the following location: $swaggerDefinitionPath"
}

function Replace-DefaultSwaggerInformation ([string]$swaggerDefinitionPath, [string]$apiDefaultUrl, [string]$apiUrl, [string]$apiDefaultName, [string]$apiName) { 
    $swaggerDefinition = Get-Content $swaggerDefinitionPath
    Write-Host "Original Swagger definition: $swaggerDefinition" -Verbose
    
    # Replace default webservice URL, if need be
    if($apiDefaultUrl -and $apiUrl) {
        $swaggerDefinition = $swaggerDefinition.Replace($apiDefaultUrl, $apiUrl)
        Write-Host "Default Url changed from '$apiDefaultUrl' to '$apiUrl'"
    }

    # Replace default API Name, if need be
    if($apiDefaultName -ne "" -and $apiName -ne "") {
        $swaggerDefinition = $swaggerDefinition.Replace($apiDefaultName, $apiName)
        Write-Host "API name changed from '$apiDefaultName' to '$apiName'"
    }

    Write-Host "Updated Swagger definition: $swaggerDefinition" -Verbose

    Set-Content -Path $swaggerDefinitionPath -Value $swaggerDefinition -Force
    Write-Host "Updated original Swagger file"
}

function Import-SwaggerInformation ([string]$apiManagementInstanceName, [string]$resourceGroupName, [string]$swaggerDefinitionPath, [string]$apiId, [string]$apiUrlSuffix) {
    # Create a context af the Azure Api management
    $apiManagementContext = New-AzureRmApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apiManagementInstanceName

    # Import the specified apiId
    Import-AzureRmApiManagementApi -Context $apiManagementContext -SpecificationFormat "Swagger" -SpecificationPath $swaggerDefinitionPath -Path $apiUrlSuffix -ApiId $apiId

    Write-Host "Swagger definition imported in $apiManagementInstanceName for api '$apiId'"
}

# Allow authentication when running locally
if($isLocalExecution -eq $true){
    Authenticate -SubscriptionId $subscriptionId
}

# Verify that the swagger definition exists on the specified location
Verify-SwaggerDefinitionLocation -swaggerDefinitionPath $swaggerDefinitionPath

# Replace common values in the swagger definition and overwrite it
Replace-DefaultSwaggerInformation -swaggerDefinitionPath $swaggerDefinitionPath -apiDefaultUrl $apiDefaultUrl -apiUrl $apiUrl -apiDefaultName $apiDefaultName -apiName $apiName

# Import the swagger definition in Azure API Management
Import-SwaggerInformation -apiManagementInstanceName $apiManagementInstanceName -resourceGroupName $resourceGroupName -swaggerDefinitionPath $swaggerDefinitionPath -apiId $apiId -apiUrlSuffix $apiUrlSuffix
