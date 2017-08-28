Param(
   [string]$EnrollmentNbr = "",
   [string]$Key = "",
   [string]$Month = ""
)
# access token is "bearer " and the the long string of garbage
$AccessToken = "bearer $Key"
$urlbase = 'https://ea.windowsazure.com'
$csvAll = @()

Write-Verbose "$(Get-Date -format 's'): Azure Enrollment $EnrollmentNbr"

# function to invoke the api, download the data, import it, and merge it to the global array
Function DownloadUsageReport( [string]$LinkToDownloadDetailReport, $csvAll )
{
		Write-Verbose "$(Get-Date -format 's'): $urlbase/$LinkToDownloadDetailReport)"
		$webClient = New-Object System.Net.WebClient
		$webClient.Headers.add('api-version','1.0')
		$webClient.Headers.add('authorization', "$AccessToken")
		$data = $webClient.DownloadString("$urlbase/$LinkToDownloadDetailReport")
		# remove the funky stuff in the leading rows - skip to the first header column value
		$pos = $data.IndexOf("AccountOwnerId")
		$data = $data.Substring($pos-1)
		# convert from CSV into an ps variable
		$csvM = ($data | ConvertFrom-CSV)
		# merge with previous
		$csvAll = $csvAll + $csvM
		Write-Verbose "Rows = $($csvM.length)"
		return $csvAll
}

if ( $Month -eq "" )
{
	# if no month specified, invoke the API to get all available months
	Write-Verbose "$(Get-Date -format 's'): Downloading available months list"
	$webClient = New-Object System.Net.WebClient
	$webClient.Headers.add('api-version','1.0')
	$webClient.Headers.add('authorization', "$AccessToken")
	$months = ($webClient.DownloadString("$urlbase/rest/$EnrollmentNbr/usage-reports") | ConvertFrom-Json)

	# loop through the available months and download data. 
	# List is sorted in most recent month first, so start at end to get oldest month first 
	# and avoid sorting in Excel
	for ($i=$months.AvailableMonths.length-1; $i -ge 0; $i--) {
		$csvAll = DownloadUsageReport $($months.AvailableMonths.LinkToDownloadDetailReport[$i]) $csvAll
	}
}
else
{
	# Month was specified as a parameter, so go ahead and just download that month
	$csvAll = DownloadUsageReport "rest/$EnrollmentNbr/usage-report?month=$Month&type=detail" $csvAll
}
Write-Host "Total Rows = $($csvAll.length)"

# data is in US format wrt Date (DD/MM/YYYY) and decimal values (3.14)
# so loop through and convert columns to local format so that Excel can be happy
Write-verbose "$(Get-Date -format 's'): Fixing datatypes..."
for ($i=0; $i -lt $csvAll.length; $i++) {
	$csvAll[$i].Date = [datetime]::Parse( $csvAll[$i].Date).ToString("d")
	$csvAll[$i].ExtendedCost = [float]$csvAll[$i].ExtendedCost
	$csvAll[$i].ResourceRate = [float]$csvAll[$i].ResourceRate
	$csvAll[$i].ResourceQtyConsumed = [float]$csvAll[$i].ResourceQtyConsumed	
}

# save the data to a CSV file
$filename = ".\$($EnrollmentNbr)_UsageDetail$($Month)_$(Get-Date -format 'yyyyMMdd').csv"
Write-Verbose "$(Get-Date -format 's'): Saving to file $filename"
$csvAll | Export-Csv $filename -NoTypeInformation -Delimiter ";"

