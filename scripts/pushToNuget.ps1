

$nugetKeyFilePath = "../nuget_apikey.txt" 
$nugetSource = "https://api.nuget.org/v3/index.json" # NuGet 推送地址


# 从文件读取 NuGet API Key
if (Test-Path $nugetKeyFilePath) {
    $nugetApiKey = Get-Content $nugetKeyFilePath -ErrorAction Stop
    Write-Host "已成功读取 NuGet API Key: $nugetApiKey" -ForegroundColor Green
    Write-Host "推送地址: $nugetSource" -ForegroundColor Green
}
else {
    Write-Error "未找到 NuGet API Key 文件，请检查路径：$nugetKeyFilePath"
    exit 1
}

$newVersion = Read-Host "请输入新的版本号 (例如 9.0.0)"

Get-ChildItem -Path $projectsPath -Recurse -Filter *$newVersion.nupkg | ForEach-Object {
    $nupkgFile = $_.FullName
    # Write-Host "dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource" -ForegroundColor Green
    if ($?) {
        Write-Host " $nupkgFile" -ForegroundColor Green
    }
    else {
        Write-Error "推送失败: $nupkgFile" 
        exit 1
    }
}

$confirmPush = Read-Host "是否推送版本[ $newVersion ]到 NuGet?(y/n)"
# dotnet nuget push "../src/*/bin/Release/*$newVersion.nupkg" --skip-duplicate -k $nugetKeyFilePath --source $nugetSource

if ($confirmPush -eq "y") {
    Write-Host "开始推送到 NuGet 源..." -ForegroundColor Cyan
    # dotnet nuget push "../src/*/bin/Release/*$newVersion.nupkg" --skip-duplicate -k $nugetKeyFilePath --source $nugetSource

    Get-ChildItem -Path $projectsPath -Recurse -Filter *$newVersion.nupkg | ForEach-Object {
        $nupkgFile = $_.FullName
        Write-Host "dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource" -ForegroundColor Cyan
        dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource
        if ($?) {
            Write-Host "推送成功 $nupkgFile" -ForegroundColor Green
        }
        else {
            Write-Error "推送失败: $nupkgFile" 
            exit 1
        }
    }
    Write-Host "所有包已成功推送到 NuGet 源。" -ForegroundColor Green

    Write-Host "查看 https://www.nuget.org/packages?q=IczpNet.AbpTrees" -ForegroundColor Green
    
}
else {
    Write-Host "推送到 NuGet 源已取消。" -ForegroundColor Yellow
}
