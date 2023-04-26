# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.7.0] - 2023-04-14

### Changed
- Upgraded to latest version of ExpressionFramework, causing some breaking changes in our own models.

### Changed
- Refactored code, so evaluation of values on in memory data (run-time objects) can be used separately. As a result, some classes have been renamed and/or moved, and this version is not backwards compatible with earlier versions.

## [0.6.1] - 2022-02-08

### Changed
- Refactored code, so dependency injection can be used for all classes

## [0.6.0] - 2022-02-07

### Added
- Added FileSystemSearch package, which implements file system search using queries. Supports both file metadata (filename, path) and file contents to query on.

### Changed
- Refactored code to fit FileSystemSearch on top of QueryFramework. As a result, this version is not backwards compatible with earlier versions.
