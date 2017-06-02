#
# Script.ps1
#


#Get-AzureRmResourceProvider -ListAvailable | Where-Object -FilterScript { 
#	$PSItem.RegistrationState -ne 'Registered'; }

$fileName = 'AzureProviders.md'
$date =  ([System.DateTime]::Now)
$providers = Get-AzureRmResourceProvider -ListAvailable
Set-Content $fileName "Updated on $date"
Add-Content $fileName "----"

foreach ($provider in $providers) {
    $txt = $provider.ProviderNamespace
	Add-Content $fileName "### $txt"
    $txt = $provider.RegistrationState
    Add-Content $fileName "$txt"
    Add-Content $fileName "   "

	foreach($resourceType in $provider.ResourceTypes) {
        $txt = $resourceType.ResourceTypeName
		  Add-Content $fileName "* $txt"
	}
    Add-Content $fileName "   "
}
