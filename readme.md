# FaceAttendance (SpeedyAPI)

Webcam-based classroom attendance using face recognition, built as an ASP.NET Core MVC web app.

![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![.NET 5](https://img.shields.io/badge/.NET-5.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4?logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)

## Overview

A teacher logs in, picks a class, and students walk past the webcam one by one. The browser keeps sending webcam snapshots to the server. The server gets a face embedding for each snapshot from a face-recognition REST service and matches it against the enrolled students' photos. Matched students are checked in (and later checked out) live, and the teacher saves the class attendance at the end.

On top of that there is a small licensing model: a school needs a valid, unused, unexpired license key to register. After that, the school manages its own teachers, students, courses and subjects.

## Features

- **Live webcam recognition.** [webcam.js](https://github.com/jhuckaby/webcamjs) snapshots are posted to the server in a loop.
  - The server asks the face service for a 512-dimensional embedding of each snapshot.
  - It picks the closest enrolled student by Euclidean distance and rejects matches at a distance of 1.0 or more.
  - A second endpoint returns the snapshot with face detections drawn on it, which is shown next to the camera feed.
- **Check-in / check-out tracking.** The first match records check-in. A later match records check-out, but only once 30 seconds have passed since the course's start time. The attendance list refreshes over AJAX, and the teacher saves it in one click.
- **Role-based areas** use session-based auth and custom MVC action filters (`AdminFilter`, `SchoolAdminFilter`, `TeacherFilter`):
  - **Admin:** create, edit and delete license keys (`school_key` / `api_key` types, with create and expiry dates).
  - **School admin:** register with a key, then manage majors/courses (with a start date), subjects (room, observing teacher), teacher accounts, students (with face photo upload) and attendance lists.
  - **Teacher:** log in, choose a major and subject, and run webcam attendance.
- **Data layer:** Entity Framework Core 5 on SQL Server, plus a full SQL script (`FaceAttendance.sql`) that creates the schema.

## Tech stack

| Layer | Technology |
| --- | --- |
| Web framework | ASP.NET Core MVC on .NET 5 (`net5.0`) |
| ORM / database | Entity Framework Core 5.0.4, SQL Server (Express) |
| Front end | Razor views, Bootstrap, jQuery, jQuery Validation, Select2, webcam.js |
| Face recognition | External REST service with an [InsightFace-REST](https://github.com/SthPhoenix/InsightFace-REST)-style API (`/extract`, `/draw_detections`, `/info`) |

## Project structure

```
FaceAttendance.sql            SQL Server schema script (UTF-16)
SpeedyAPI.sln
SpeedyAPI/
├── Controllers/              Admin, Keys, SchoolAccounts, Majors, Subjects,
│                             TeacherAccounts, Students, Attendances, WebcamAttendance
├── Data/                     EF Core DbContexts (one per aggregate)
├── Filters/                  Session-based authorization action filters
├── Models/                   Entities + WebcamAttendance view models (face vectors, room)
├── Validations/              Custom validation attributes
├── Views/                    Razor views per controller
└── wwwroot/                  Static assets, webcam.js, uploaded student photos (upload/)
```

## Getting started

### Prerequisites

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- SQL Server or SQL Server Express
- A face-recognition service with an InsightFace-REST-compatible API, reachable at `http://localhost:18081`

### Setup

1. **Create the database.** Run `FaceAttendance.sql` in SQL Server Management Studio (or `sqlcmd`). It creates the `FaceAttendance` database and its tables.
2. **Configure the connection string.** In `SpeedyAPI/appsettings.json`, point `ConnectionStrings:speedy` at your SQL Server instance.
3. **Start the face service** on port `18081`. The URL is set in the `insightFacelURL` field of `Controllers/WebcamAttendanceController.cs`, so change it there if your service runs somewhere else.
4. **Run the app:**

   ```bash
   cd SpeedyAPI
   dotnet run
   ```

   Then open `https://localhost:5001`.

> The admin login is hardcoded in `Controllers/AdminController.cs`, and accounts are stored in plain text. Change this before deploying anywhere public.

## Usage

1. **Admin** (`/Admin`): create a `school_key` license key.
2. **School** (`/Keys/Use`): enter the key and create the school account.
3. **Set up the school.** Log in as the school and add majors, subjects, teachers and students (each with a clear face photo). Then add students to a subject's attendance list.
4. **Take attendance.** The teacher opens `/WebcamAttendance`, logs in, picks the major and subject, clicks **Start recognition**, lets students step in front of the camera, and clicks **Save Attendances** when done.

## Project history

The first plan was to build the recognition backend on [Dlib](http://dlib.net/) compiled with CUDA/cuDNN, behind a REST API. The finished version calls an InsightFace-style REST service for detection and embeddings instead. A paid API-only plan (daily request quotas via `api_key` licenses) was also planned; the key type exists in the data model, but no API endpoint uses it.

## Team

- [khangzxrr](https://github.com/khangzxrr) (Vo Ngoc Khang)
- [sangxyz](https://github.com/sangxyz)
