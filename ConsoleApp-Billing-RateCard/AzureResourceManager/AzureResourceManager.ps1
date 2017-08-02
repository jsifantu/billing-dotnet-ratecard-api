param ([string]$accountName)

$accountName = "jdsazuregroupdiag532"
$accountKey = "IenvG/Ja+bRmjKxPkeIQyfs/9/u8BNPzUp29mSHvPsvFZZ9ypfVDSb+SUl03iub5MhdYRUW4UMKH/mk8wLHNdg=="
# $ctx = New-AzureStorageContext -StorageAccountName $accountName -StorageAccountKey $accountKey

$accounts = Add-AzureAccount
#$accounts = Get-AzureAccount
foreach ($account in $accounts) {
    Write-Host -ForegroundColor Yellow 'Account ID: ' $account.Id
    Write-Host -ForegroundColor White '  Subscription'
    foreach ($subId in $account.Subscriptions) {
        Write-Host -ForegroundColor Cyan '    ID: ' $subId
		$subscription = Get-AzureSubscription 
		$subscription
	}
	Write-Host -ForegroundColor White '  Tenants'
	foreach ($tenantId in $account.Tenants) {
		Write-Host -ForegroundColor Cyan '    ID: ' $tenantId
	}
}

Remove-AzureAccount

