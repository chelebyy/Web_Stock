---
description: 
globs: 
---
# Inventory and Stock Tracking Web Application - Product Requirements Document (PRD)

---

## 1. Overview

### 1.1 Project Summary
The Inventory and Stock Tracking Web Application is designed to manage and monitor inventory levels and stock movements across various departments. The application will operate solely on a local network (no internet access) and is accessible only via desktop web browsers. The system is built using .NET Core (backend), PostgreSQL (database), Angular 19.1.2 (frontend), and the PrimeNG library (UI components).

### 1.2 Goals and Objectives
- **Secure Inventory Management:** Provide a secure system for tracking inventory and managing stock data.
- **Role-Based Access:** Ensure that users only see and interact with pages and functionalities for which they have permissions.
- **Auditability:** Maintain comprehensive logs of user activities for accountability and auditing.
- **Data Security:** Protect sensitive user information by encrypting passwords.
- **Local Operation:** Guarantee that the system operates completely on a local network with no external internet dependencies.
- **Desktop-Only Access:** Restrict access exclusively to desktop computers to maintain system performance and data integrity.

---

## 2. Stakeholders


- **Development Team:** .NET Core Developers, Angular Developers, Database Administrators
- **System Administrators:** Responsible for managing user accounts, roles, and permissions
- **End Users:** Inventory managers, IT staff, and department-specific users (e.g., Health, Penitentiary)

---

## 3. User Roles and Permissions

- **Admin:**
  - Create, update, and delete user accounts.
  - Manage role assignments and permissions.
  - Access all system pages, including audit logs and configuration settings.
  
- **Standard User:**
  - Perform data operations (add, update, delete) on assigned pages.
  - Access is limited to pages explicitly granted via permissions.
  - No access to admin-level functionalities.

*Note: Each user's permissions are granular and page-specific, ensuring users can only interact with the modules they are authorized to use.*

---

## 4. Features & Requirements

### 4.1 User Management
- **Account Creation:**  
  - Admins or designated personnel can create new user accounts.
  - Account creation includes assigning appropriate roles and page access permissions.

- **Password Management:**  
  - Users can change their passwords after logging in.
  - Passwords must be stored in an encrypted form in the database following industry-standard encryption protocols.

### 4.2 Multiple Pages and Access Control
- **Pages Overview:**  
  - The system will initially include pages such as:
    - **Information Technology**
    
  - The architecture must support the addition of new pages as needed.

- **Dynamic Navigation:**  
  - Upon login, only the buttons/links corresponding to the pages the user has permissions for should be visible.
  
### 4.3 Log Tracking
- **Activity Logging:**  
  - Log all significant user activities, including:
    - Login attempts and status.
    - Data changes (additions, updates, deletions).
    - Changes in permissions or configurations.
  - Logs must be securely stored and accessible to authorized personnel for auditing purposes.

### 4.4 Password Encryption
- **Security Measures:**  
  - All user passwords will be encrypted using robust, industry-standard encryption algorithms.
  - Enforce strong password policies regarding complexity and length.

### 4.5 Local Network Operation
- **Local Environment:**  
  - The application must operate solely within the local network environment.
  - No external data pulls or internet-based integrations are allowed.
  - Ensure that all necessary libraries and resources are hosted locally.

### 4.6 Device Restrictions
- **Desktop-Only Access:**  
  - The system is designed exclusively for desktop web browsers.
  - Mobile phones and tablets are not supported to maintain consistency and data security.
  
---

## 5. Technical Specifications

- **Backend Framework:** .NET Core 9
- **Database:** PostgreSQL
- **Frontend Framework:** Angular 19.1.2
- **UI Components:** PrimeNG Library
- **CSS Framework:** Tailwind CSS 3.4.1
- **Hosting Environment:** Local server within the organization's network

---

## 6. Constraints and Considerations

- **Internet Isolation:**  
  - The system must function without any internet access. All dependencies must be locally hosted.
  
- **Hardware Compatibility:**  
  - Testing must cover major desktop browsers (e.g., Chrome, Firefox, Edge) to ensure compatibility.
  
- **Scalability:**  
  - While the initial version supports specific pages, the design must allow for future expansion and feature additions.

- **Security Compliance:**  
  - The system must adhere to internal security standards for local applications, including encryption and audit logging.

---

## 7. Acceptance Criteria

- **Deployment:**  
  - The application is successfully deployed on the local network without internet access.
  
- **User Functionality:**  
  - Admins can create and manage user accounts, including role and permission assignments.
  - Standard users can access and interact with only the pages they are permitted to view.
  
- **Security:**  
  - Passwords are stored in an encrypted format.
  - Comprehensive logs of user activities are maintained and accessible to authorized users.
  
- **UI/UX:**  
  - The navigation dynamically displays only the pages that the user is authorized to access.
  
- **Device Restriction:**  
  - The application is accessible only via desktop browsers, with mobile and tablet access blocked.

---

## 8. Future Enhancements

- **Mobile Support:**  
  - Evaluate the feasibility of mobile and tablet support based on evolving business needs.
  
- **Advanced Reporting:**  
  - Incorporate analytics and reporting tools for in-depth inventory insights.
  
- **Integration Capabilities:**  
  - Consider future integrations with additional internal systems or databases as required.

---

## 9. Glossary

- **Admin:** A user with full permissions, including user management and system configuration.
- **Standard User:** A user with restricted permissions, limited to specific pages and functions.
- **Local Network:** An internal network environment with no access to the public internet.
- **Encryption:** The process of converting data (passwords) into a secure format that cannot be easily understood without decryption.

---

## 10. Revision History

| Date       | Version | Description                   | Author      |
|------------|---------|-------------------------------|-------------|
| 2025-02-15 | 1.0     | Initial PRD Draft             | [Your Name] |

## 11. Semantic coding rules

  Overall Guidelines:

Use meaningful names for variables, functions, classes, and database objects
Follow consistent casing conventions across technologies
Implement clear patterns for common operations
Document complex logic and architecture decisions

.NET Core Backend:

Use Pascal Case for class names, properties, and public methods (e.g., UserService, GetUserById)
Use camel case for private fields and local variables (e.g., private string userName;)
Follow repository pattern for database access
Use dependency injection consistently
Implement service interfaces (e.g., IUserService) for testability
Group related functionality into appropriate namespaces
Use async/await for database operations

Angular/TypeScript Frontend:

Follow Angular style guide conventions
Use descriptive component names with Component suffix (e.g., UserProfileComponent)
Create reusable services with Service suffix (e.g., AuthenticationService)
Implement typed models that match backend DTOs
Use interfaces for complex object definitions
Create standalone modules for major feature areas
Use reactive forms pattern for complex forms

PrimeNG Components:

Create wrapper components for frequently used PrimeNG components
Maintain consistent styling patterns
Document component usage in comments
Structure template layouts consistently

Tailwind CSS Styling:

- Follow utility-first approach consistently
- Create custom utility classes only when necessary
- Use @apply directive for complex, repeating utility combinations
- Maintain a consistent color palette and spacing scale
- Document custom Tailwind configurations
- Use responsive design utilities systematically
- Create reusable component classes for common patterns

PostgreSQL Database:

Use snake_case for table and column names (e.g., user_profile, first_name)
Create proper indexes for frequently queried columns
Use appropriate data types for performance optimization
Implement consistent naming for constraints and sequences
Document complex queries with comments

---

*This PRD is intended to serve as a comprehensive guide for the development and deployment of the Inventory and Stock Tracking Web Application. Feedback and revisions will be incorporated as the project progresses.*



