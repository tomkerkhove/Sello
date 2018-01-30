Param(
    [string] $connectionString = $(throw "No connection string was specified"),
    [string] $databaseContextDll = $(throw "No path was specified to the DLL that contains the database context"),
    [string] $migrateToolPath = $(throw "No path was specified to migrate.exe")
)

function Verify-MigrationToolIsAvailable ([string]$migrateToolPath) {
    if(!(Test-Path $migrateToolPath)) {
        Throw [System.IO.FileNotFoundException] "Migration tool was not found at $migrateToolPath"
    }

    Write-Host "Migration tool exists on the following location: $migrateToolPath"
}

function Execute-MigrationTool ($commandTitle, $commandPath, $commandArguments) {
    $processInfo = New-Object System.Diagnostics.ProcessStartInfo
    $processInfo.FileName = $commandPath
    $processInfo.RedirectStandardError = $true
    $processInfo.RedirectStandardOutput = $true
    $processInfo.UseShellExecute = $false
    $processInfo.CreateNoWindow = $true
    $processInfo.Arguments = $commandArguments
    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $processInfo
    $process.Start() | Out-Null
    $process.WaitForExit()

    [pscustomobject]@{
        Process = $process
    }
}

function Migrate-Database ([string]$migrateToolPath, [string]$connectionString, [string]$databaseContextDll) {
    
    $databaseContextDllFolder = Split-Path -Path $databaseContextDll
    $copiedMigrateToolPath = [System.IO.Path]::Combine($databaseContextDllFolder, "migrate.exe");
    Copy-Item "$(Split-Path -Path $migrateToolPath)/*" $databaseContextDllFolder
    
    $databaseContextDllName = [System.IO.Path]::GetFileNameWithoutExtension($databaseContextDll) + [System.IO.Path]::GetExtension($databaseContextDll)
    $toolArguments = """$databaseContextDllName"" /connectionString=""$connectionString"" /connectionProviderName=""System.Data.SqlClient"" /verbose"
    Write-Output "Invoking Migrate.exe using arguments $toolArguments for tool $copiedMigrateToolPath" 

    $migrationProcess = Execute-MigrationTool "Run Migrate.exe" $copiedMigrateToolPath $toolArguments
    if($migrationProcess.Process.ExitCode -ne 0) {
        $failureMessage = "Database failed to migrate"
        if($migrationProcess.Process.StandardOutput -ne $null){
            $failureMessage += ". Reason: - $($migrationProcess.Process.StandardOutput.ReadToEnd())"
        }
        
        Write-Error $failureMessage
    }else{
        Write-Host "Database was successfully migrated"
    }
}

Verify-MigrationToolIsAvailable -migrateToolPath $migrateToolPath
Migrate-Database -migrateToolPath $migrateToolPath -connectionString $connectionString -databaseContextDll $databaseContextDll
 