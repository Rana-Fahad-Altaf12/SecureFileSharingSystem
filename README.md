🔐 Secure File Sharing System
A full-stack application built with ASP.NET Core API and Angular for secure file storage and sharing, including JWT authentication, AES encryption, REST API access, and audit logging.

🏗️ Architecture Overview
Backend – ASP.NET Core Web API
Layered Clean Architecture

Features:

JWT Authentication (Login/Register)

AES Encrypted File Storage

RESTful File Upload/Download

Third-party API Key secured downloads

EF Core with Migrations


🧱 Project Structure:

/API – ASP.NET Core Web API project

/Application – Business logic layer (Use Cases, Interfaces)

/Domain – DTOs

/Infrastructure – File encryption/storage, DB, Auth services

/Persistence – EF Core DbContext, Migrations


Frontend – Angular 18
Standalone Components

Features:

Login/Register UI with loader and error display

File Upload with feedback

File List View + Download button

Dynamic Menu:

Hidden on Login/Register

Shows Logout on Auth

Third-party download via API key (no login required)

🧱 Project Structure:

/src/app/

/core/services – AuthService, FileService, etc.

/auth – login.component.ts, register.component.ts

/upload – upload.component.ts

/download – download.component.ts

/third-party – third-party.component.ts

/layout – shared menu/navbar

/app-routing.module.ts – route guards & paths

🚀 Features & Use Cases
Feature	Description
🛂 JWT Auth	Secure login/register with token storage in localStorage
🔒 AES File Encryption	Files encrypted before saving to disk
☁️ Upload / Download	Files uploaded/downloaded securely
📜 File Listing	Authenticated users can see and download their uploaded files
🧪 Third-party API Access	Public users can download via File ID + API Key

📸 UI Flow
Login/Register

Show loader on click, disable button

Show Register/Forget password links

Upload Page

Select + Upload file

Show loader & success/error

File List

Authenticated view of uploaded files

Download buttons

Third-party Page

No login required

API Key + File ID → Download file


🔐 Security
Passwords hashed with ASP.NET Identity

Tokens expire, stored client-side

API Key-based file download for third-parties (secure + auditable)

AES file encryption at rest

Routes protected with Auth Guards (Angular) and [Authorize] (API)

📊 Tech Stack
Layer	Tech
Frontend	Angular 18, Bootstrap, RxJS
Backend	ASP.NET Core 6, EF Core
Auth	JWT + ASP.NET Identity
Encryption	AES with custom IEncryptionService
Storage	FileSystem
Database	SQL Server