# ApiAutomation (Visual Studio 2022) - Data-driven CRUD Tests (NUnit)
## Overview
This solution provides a **Page-Object-Model (POM)** style API test automation framework targeting the public test API:  
👉 [https://api.restful-api.dev](https://api.restful-api.dev)

It is built to be **reviewer-friendly**, with detailed inline comments and separate, independent tests for each CRUD operation.

---

## Contents
- **Base/** – `BaseApi.cs` (shared RestSharp client & helpers)  
- **Config/** – `appsettings.json`, `ConfigReader.cs` (supports `BASE_URL` environment variable)  
- **Endpoints/** – `ObjectEndpoint.cs` (POM-style CRUD methods)  
- **Models/** – `CreatePayload`, `PatchPayload`, `ObjectModel`  
- **Tests/** – `PostTests`, `GetTests`, `PutTests`, `PatchTests`, `DeleteTests`, `NegativeTests`, `TestBase`  
- **.github/workflows/dotnet.yml** – GitHub Actions workflow  
- **ApiAutomation.sln / ApiAutomation.csproj** – Solution and project files  
- **testdata/** Test data is stored in the `Data/testdata.json` file and deserialized at test runtime.
---

## How Tests Are Designed (Important for Reviewers)
- **Independence**: Every test creates its own data where required. There are no hardcoded GUIDs or shared IDs.  
- **Clarity**: Each test method includes `Arrange / Act / Assert` sections and inline comments.  
- **Negative Cases**: Tests include invalid input, malformed IDs, and null-body checks.  
- **POM Principle**: Endpoints encapsulate HTTP details so that tests remain human-readable.  

---

## Run Locally (Visual Studio 2022)
1. Open `ApiAutomation.sln` in **Visual Studio 2022**.  
2. Restore NuGet packages:  
   - Go to **Tools → NuGet Package Manager → Package Manager Console**  
   - Run:  
     ```powershell
     Update-Package -reinstall
     ```  
3. Open **Test Explorer** and run the tests.  
4. To override the base URL for CI, set the environment variable `BASE_URL` to your desired URL before running tests.

---

## GitHub Actions (CI)
A sample workflow file `.github/workflows/dotnet.yml` is included.  
It runs on **windows-latest** runners and executes the following command to produce TRX results:
```bash
dotnet test
