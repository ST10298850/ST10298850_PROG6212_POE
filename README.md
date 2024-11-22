# ST10298850_PROG6212_POE  
## **Contract Monthly Claim System (CMCS)**

The **Contract Monthly Claim System (CMCS)** is a web-based application designed to streamline the process of submitting and reviewing monthly claims for Independent Contractor (IC) lecturers. The system offers a transparent and efficient workflow, enabling lecturers to submit claims, coordinators to verify them, academic managers to finalize decisions, and HR users to manage payroll integration.  

---

## **Features**

- **Claim Submission**:  
  Lecturers can submit claims, including details of hours worked, hourly rate, and additional notes. Supporting documents can also be uploaded. Real-time status updates are available for tracking claims.

- **Enhanced Verification Process**:  
  Coordinators can review claims to verify details and approve or reject them, adding comments or feedback where necessary.

- **Final Approval Workflow**:  
  Academic Managers review coordinator-approved claims and provide the final decision, ensuring a two-layer review process.

- **Payroll Integration**:  
  HR users can access approved claims to process payments efficiently and manage payroll data.

- **Role-Based Access Control (RBAC)**:  
  Authentication and session management are implemented to provide secure access for Lecturers, Coordinators, Academic Managers, and HR users, each with role-specific capabilities.

- **Session Management**:  
  User sessions track role-based access, securely storing session details for authenticated users.

- **Notes Field**:  
  A mandatory "Notes" field is included to capture additional claim details and facilitate communication between roles.

---

## **User Roles**

1. **Lecturer**:  
   - Submit claims for hours worked.  
   - Upload supporting documents.  
   - Monitor the status of claims.  
   - Add mandatory notes for additional context.

2. **Coordinator**:  
   - Review and verify claims submitted by lecturers.  
   - Approve or reject claims with comments.  
   - Manage claims assigned to their department.

3. **Academic Manager**:  
   - Perform the final review of claims verified by coordinators.  
   - Approve or reject claims.  
   - View reports and summaries of verified claims.

4. **HR User**:  
   - Access all approved claims for payroll processing.  
   - Generate payroll reports.  
   - Ensure timely disbursement of payments.

---

## **System Architecture**

- **Frontend**:  
  HTML5, CSS3, JavaScript (jQuery, AJAX) for an interactive user experience and dynamic data updates.

- **Backend**:  
  ASP.NET Core MVC, C# for robust and scalable server-side functionality.

- **Database**:  
  SQL Server (LocalDb) managed using Entity Framework Core. Includes updated tables for efficient tracking of roles, claims, and the approval process.

- **Security**:  
  Passwords are hashed using SHA-256. Secure sessions ensure confidentiality and integrity.

---

## **Database Structure**

1. **Claims**:  
   Stores details of lecturer claims, including:  
   - Hours worked.  
   - Hourly rate.  
   - Status (e.g., Pending, Verified, Approved, Rejected).  
   - Notes for additional information.  
   - Submission and verification dates.

2. **Users**:  
   Contains information about system users, including:  
   - Role (Lecturer, Coordinator, Academic Manager, HR).  
   - Securely hashed passwords.  

3. **Approvals**:  
   Tracks the verification and approval stages of each claim, including timestamps and reviewer comments.

4. **Documents**:  
   Handles file uploads for claims, storing paths to supporting documents.

---

## **Installation Guide**

1. Ensure the following prerequisites are installed:
   - .NET SDK.  
   - SQL Server (LocalDb).

2. Clone this repository using:
   ```bash
   git clone <repository-url>
