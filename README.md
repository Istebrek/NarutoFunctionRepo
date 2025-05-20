# Naruto Characters Function app API on Azure

This is a serverless REST API built with Azure Functions and backed by Cosmos DB. It allows you to manage Naruto characters with CRUD operations, because spreadsheets are too slow for ninja data.

## What This Does (Allegedly)

- Lets you **POST**, **GET**, **DELETE**, and **UPDATE** anime characters.
- Stores them in Cosmos DB, because using SQL Server would be too mainstream.
- Secured with Azure Function keys. Hackers hate this one weird trick.
- Designed with REST principles, because we’re not savages.
- Tested using Postman. Nothing says "real developer" like sending HTTP requests to yourself at 2AM.

## Table of Contents

- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Security](#security)
- [Tech Stack](#tech-stack)
- [How to Run Locally](#how-to-run-locally)

## Getting Started

Clone the repo:

```bash
git clone https://github.com/Istebrek/NarutoFunctionRepo.git
```

#### Make sure you have the following installed:
- Azure Functions Core Tools
- Visual Studio 2022 or VS Code
- Postman (for testing)
- MongoDB-compatible connection string

## API Endpoints
#### **GET** /api/GetCharacters?code=function-key
Returns a list of all characters. Filler characters included — no discrimination.

#### **GET** /api/GetCharacter/{id}?code=function-key
Returns a single character by GUID. If the character is hiding like Tobi, returns an error.

#### **POST** /api/PostCharacter?code=function-key
Adds a new character to the database.

**Request body (JSON):**
```json
{
  "name": "Naruto Uzumaki",
  "village": "Leaf",
  "rating": 12,
  "jutsu": "Shadow Clone"
}
```

Alternatively, query string format:
```bash
/api/PostCharacter?code=<your-function-key>&name=Naruto&village=Leaf&rating=12&jutsu=Shadow%20Clone
```

#### **DELETE** /api/DeleteCharacter/{id}?code=function-key
Deletes a character by GUID. Permanent — no reanimations allowed.

#### **POST** /api/UpdateCharacter/{id}?code=function-key
Updates the character's data. Think of it like Naruto's character development, but faster.

Request body (JSON):

```json
{
  "name": "Naruto",
  "village": "Leaf",
  "rating": 10000,
  "jutsu": "Sage Mode"
}
```

## Security
This API uses Azure Function keys for protection. If you try accessing endpoints without the ?code=... query parameter, you’ll get rejected faster than Sakura asking Sasuke out.

#### Make sure to:
* Copy the function key from Azure Portal
* Use it like this:
```bash
GET /api/GetCharacters?code=function-key
```

## Tech Stack
* Azure Functions (.NET Isolated)
* Cosmos DB (MongoDB)
* C#
* Postman (for testing)

## How to Run Locally
1. Clone the repo
2. Add your Cosmos DB connection string (you're obvs not getting mine) to local.settings.json:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "CosmosString": "your-mongodb-connection-string"
  }
}
```
3. Build and run the project
4. Hit the endpoints using Postman or your browser


![funny-memes-anime-coding-20](https://github.com/user-attachments/assets/dccebf10-9bc1-4c15-adcc-18e059c34357)

