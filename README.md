# LrndefLib
LrndefLib provides a version-based config file

[![GitHub license](https://img.shields.io/github/license/Arthri/LrndefLib?style=flat-square "License")](../../blob/HEAD/LICENSE)
[![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/Arthri/LrndefLib?sort=semver&style=flat-square "Latest GitHub Release")](../../releases/latest)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/Arthri/LrndefLib?include_prereleases&sort=semver&style=flat-square "Latest GitHub Release(including Pre-Releases)")](../../releases)
[![NuGet](https://img.shields.io/nuget/v/LrndefLib?style=flat-square "Latest NuGet Release")](https://www.nuget.org/packages/LrndefLib/latest)
[![NuGet (with prereleases)](https://img.shields.io/nuget/vpre/LrndefLib?style=flat-square "Latest NuGet Release(including Pre-Releases)")](https://www.nuget.org/packages/LrndefLib/absoluteLatest)

## Goals

### Version Ambiguity
The config file implementation provided by TShock can't differentiate between config versions without iterating the whole file. LrndefLib provides an alternative implementation that instead compares a single property "metadata:version". However, the schema isn't strictly enforced

### Migrations
LrndefLib allows for config migrations during parsing. However, you need to provide your own migration implementation. LrndefLib can't migrate by default, it binds the config 1:1 as if it were any other version

Migrations can be implemented by modifying `VersionedConfigFile.BindDelegate`. The delegate is called in the middle of parsing to bind a `JObject` to a config object

## Installation

### With Visual Studio/NuGet
1. Open NuGet
2. Search for "LrndefLib"
3. Install

### With Paket(HTTP) (Old Way)
1. Add `http https://github.com/Arthri/LrndefLib/releases/download/[VERSION]/LrndefLib.[VERSION].nupkg packages/LrndefLib/[VERSION]/LrndefLib.[VERSION].nupkg` to your `paket.dependencies`
    - Replace `[VERSION]` with your desired version. **DO NOT** use `latest`.
2. Add `source paket-files/github.com/packages`, also to `paket.dependencies`
3. Reference in projects via `paket.references`
4. Run `dotnet paket install`

## Usage
1. Create a new instance of `VersionedConfigFile` like so: `var configFile = new VersionedConfigFile<TSettings>(CurrentVersion);`
    - replace `TSettings` with the settings class
    - replace `CurrentVersion` with the current version of the config
2. Read the config file: `configFile.Read(configPath, out bool _)`;

## Unofficial Conventions

### CurrentVersion
A config's "`CurrentVersion`" is its latest iteration. Ideally, it should follow [SemVer][SemVer]. Since configs change rarely, ideally, `CurrentVersion` should NOT be derived from the assembly version





<!-- DEFINITIONS -->
[SemVer]: https://semver.org/
