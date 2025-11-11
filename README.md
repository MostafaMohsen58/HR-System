# HR Management System

## Overview
The **HR Management System** is a web-based application designed to streamline and automate HR operations.  
It provides secure authentication, employee data management, attendance tracking, payroll processing, and holiday configuration.  
This system aims to improve efficiency, reduce manual errors, and centralize HR processes in one platform.

---

## Features

### 👩‍💼 HR Accounts
- Secure **JWT Authentication** for HR users.
- Role-based access control (RBAC) to manage permissions.
- Login system with token storage for session management.

### 👤 Employee Management
- Add, update, retrieve, and delete employee records.
- Manage contract details, working hours, and personal data.

### 🧾 Payroll Management
- Generate and view payroll reports.
- Calculate salary based on:
  - Presence & absence
  - Extra hours
  - Deductions

### 🕒 Attendance Tracking
- Record daily attendance with arrival & departure times.
- Filter attendance by date or employee.

### 📆 Official Holidays
- Manage holiday names and dates.
- Integrated with attendance and payroll modules.

### ⚙️ Settings
- Configure global settings such as:
  - Holiday setup
  - Additions & deductions rules

---

## Tech Stack

### Backend
- **.NET 9.0**
- **Entity Framework Core**
- **JWT Authentication**
- **Hangfire** for background jobs

### Database
- **SQL Server**

---

## 📄 API Documentation
Full API documentation is available here:  
[📄 View API Docs](https://documenter.getpostman.com/view/46232846/2sB34hG1JX)

---

## 🔑 Authentication
- Authentication is handled using **JWT**.
- To access secured endpoints:
  1. **Login** with HR credentials using `/Accounts/login`.
  2. Copy the returned **JWT token**.
  3. Include it in the `Authorization` header for subsequent requests:
     ```http
     Authorization: Bearer <your_token>
     ```

---

## Installation & Setup

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/your-username/hr-management-system.git
cd hr-management-system
```

### 2️⃣ Configure the Database
* Update the appsettings.json file with your SQL Server connection string:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HRSystem;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
* Add your HuggingFace token:
```bash
"HuggingFace": {
  "Token": "your_token_here"
}
```

### 3️⃣ Apply Migrations
```bash
dotnet ef database update
```
### 4️⃣ Run the Application
```bash
dotnet run
```

---

## Default Roles

HR Manager: Full system access.

User: View personal data & attendance.

---

## Screenshots
### Dashboard Overview
![Dashboard View](https://drive.google.com/uc?export=view&id=1sQxWK_FC2zIt-kKbMmmCWO6DwnIeF4_o)

### Roles Page
![Roles Page](https://drive.google.com/uc?export=view&id=1cIn_CBIA7TO_rEMCiAlYWGJUrk94_uIc)

### Users List Page
![Users Page](https://drive.google.com/uc?export=view&id=1orB2bUqJk43VB-FUE59rwetjd-3Gmyia)

### Employee List Page
![Employee List](https://drive.google.com/uc?export=view&id=1I14UlOaRx8HtDQtes2qqtuyoMAY3vt1_)

### Attendance Page
![Attendance Page](https://drive.google.com/uc?export=view&id=1jq7fgArijh6CvBX069QNHdn59HyST2xD)

### Salary Report Page
![Report Page](https://drive.google.com/uc?export=view&id=1XSXpwRVkCaO04bMnhJgYMCcsCvfIFDK8)

#### Responsive Example
![Attendance Mobile View](https://drive.google.com/uc?export=view&id=17cRZobedaaB77Nujsv7dKJW5aNVFJgJd)

---

## Demo Video

[![Watch the video](https://drive.google.com/uc?export=view&id=10Cex7cojWBHroqAqBI6aQsmj_wix7N5g)](https://vimeo.com/1109262465)
