Framework 4.5.1

Properties {
    $base_dir = Split-Path $psake.build_script_file	
    $out_dir = "$base_dir\out"
    $nuget_dir = "$base_dir\dist"
    
    $nunit = "$base_dir\packages\NUnit.ConsoleRunner.3.2.1\tools\nunit3-console.exe"
    
    $solution = "$base_dir\EmbeddedFileReader.sln"
    $project = "$base_dir\EmbeddedFileReader\Source.csproj"
    $configuration = "Release"
    $platform = "Any CPU"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends Build

Task Build -Depends Clean, Compile, Test, NugetPack

Task Clean {
    Write-Host "Creating artifacts directory..." -ForegroundColor Green

    create_directory $out_dir
    
    Write-Host "Cleaning..." -ForegroundColor Green
    Exec { msbuild $solution /t:Clean /p:Configuration=$configuration /p:Platform=$platform /v:quiet } 
}

Task Compile -Depends Clean {	
    Write-Host "Building..." -ForegroundColor Green
    Exec { msbuild $solution /t:Build /p:Configuration=$configuration /p:Platform=$platform /v:quiet /p:OutDir=$out_dir } 
}

Task Test -Depends Compile {
    $assemblies = (Get-ChildItem $out_dir -Recurse -Include *Tests.dll)
    run_nunit $assemblies
}

Task NugetPack -Depends Test {
    Write-Host "Creating nuget package..." -ForegroundColor Green

    create_directory $nuget_dir
    
    Exec { nuget pack $project -Prop OutDir=$out_dir -sym -OutputDirectory $nuget_dir }
}

#-- Helper functions

function global:create_directory($directory)
{
    if (Test-Path $directory) 
    {	
        rd $directory -rec -force | out-null
    }
    
    mkdir $directory | out-null
}

function global:run_nunit ($test_assembly)
{
    Write-Host "Running tests..." -ForegroundColor Green
    exec { & $nunit $test_assembly --work=$out_dir --noresult }
}