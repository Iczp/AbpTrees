# 配置参数
$projectsPath = "." # 替换为你的解决方案路径
$nugetKeyFilePath = "../nuget_apikey.txt" 
$nugetSource = "https://api.nuget.org/v3/index.json" # NuGet 推送地址

# 检查是否有未提交的 Git 更改
Write-Host "检查是否有未提交的 Git 更改..." -ForegroundColor Cyan
cd $projectsPath
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "检测到未提交的更改，请先提交或暂存以下文件：" -ForegroundColor Red
    Write-Host $gitStatus
    # exit 1
} else {
    Write-Host "没有未提交的 Git 更改，继续执行脚本。" -ForegroundColor Green
}

# 从文件读取 NuGet API Key
if (Test-Path $nugetKeyFilePath) {
    $nugetApiKey = Get-Content $nugetKeyFilePath -ErrorAction Stop
    Write-Host "已成功读取 NuGet API Key: $nugetApiKey" -ForegroundColor Green
} else {
    Write-Error "未找到 NuGet API Key 文件，请检查路径：$nugetKeyFilePath"
    exit 1
}

# 手动输入版本号
$newVersion = Read-Host "请输入新的版本号 (例如 1.0.1)"
if (-not $newVersion) {
    Write-Error "版本号不能为空！"
    exit 1
}

# 1. 查找并更新项目版本号
Write-Host "正在查找 .csproj 文件并更新版本号..." -ForegroundColor Cyan
Get-ChildItem -Path $projectsPath -Recurse -Filter *.csproj | ForEach-Object {
    $file = $_.FullName
    # 更新 TargetFramework 到 net9.0
    (Get-Content $file) -replace '<TargetFramework>net8.0<\/TargetFramework>', '<TargetFramework>net9.0</TargetFramework>' | Set-Content $file
    # 更新版本号
    if ((Get-Content $file) -match '<Version>') {
        (Get-Content $file) -replace '<Version>.*<\/Version>', "<Version>$newVersion</Version>" | Set-Content $file
        Write-Host "已更新版本号: $file" -ForegroundColor Green
    } 
    # else {
    #     # 如果没有 <Version> 标签，添加它
    #     $content = Get-Content $file
    #     $index = $content.IndexOf('</PropertyGroup>') # 找到第一个 PropertyGroup 结束位置
    #     $content[$index] = "  <Version>$newVersion</Version>`n$content[$index]"
    #     $content | Set-Content $file
    #     Write-Host "已添加版本号: $file" -ForegroundColor Green
    # }
}

# 2. 还原依赖项并检查构建
Write-Host "还原依赖项并构建项目..." -ForegroundColor Cyan
cd $projectsPath
dotnet restore
if ($?) {
    Write-Host "依赖项还原成功。" -ForegroundColor Green
} else {
    Write-Error "依赖项还原失败，请检查项目配置。" 
    exit 1
}

Write-Host "更新Abp" -ForegroundColor Cyan
Write-Host "abp update" -ForegroundColor Cyan
abp update
# dotnet build --configuration Release
# if ($?) {
#     Write-Host "项目构建成功。" -ForegroundColor Green
# } else {
#     Write-Error "项目构建失败，请检查代码。" 
#     exit 1
# }

# # 3. 打包 NuGet 包
# Write-Host "正在打包项目为 NuGet 包..." -ForegroundColor Cyan
# Get-ChildItem -Path $projectsPath -Recurse -Filter *.csproj | ForEach-Object {
#     $projectDir = Split-Path -Path $_.FullName -Parent
#     cd $projectDir
#     dotnet pack --configuration Release
#     if ($?) {
#         Write-Host "打包成功: $projectDir" -ForegroundColor Green
#     } else {
#         Write-Error "打包失败: $projectDir" 
#         exit 1
#     }
# }

# # 4. 用户确认是否推送到 NuGet
# Write-Host "打包已完成。准备推送到 NuGet 源。" -ForegroundColor Cyan
# $confirmPush = Read-Host "是否推送到 NuGet？输入 yes 继续，其他输入取消"

# if ($confirmPush -eq "yes") {
#     Write-Host "开始推送到 NuGet 源..." -ForegroundColor Cyan
#     Get-ChildItem -Path $projectsPath -Recurse -Filter *.nupkg | ForEach-Object {
#         $nupkgFile = $_.FullName
#         dotnet nuget push $nupkgFile --api-key $nugetApiKey --source $nugetSource
#         if ($?) {
#             Write-Host "推送成功: $nupkgFile" -ForegroundColor Green
#         } else {
#             Write-Error "推送失败: $nupkgFile" 
#             exit 1
#         }
#     }
#     Write-Host "所有包已成功推送到 NuGet 源。" -ForegroundColor Green
# } else {
#     Write-Host "推送到 NuGet 源已取消。" -ForegroundColor Yellow
# }

# Write-Host "脚本执行完成！" -ForegroundColor Cyan
