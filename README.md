# ST10298850_PROG6212_POE  
## **Contract Monthly Claim System (CMCS)**

The **Contract Monthly Claim System (CMCS)** is a web-based application designed to streamline the process of submitting and reviewing monthly claims for Independent Contractor (IC) lecturers. Lecturers can submit claims for hours worked, upload supporting documents, and track their claim status. Coordinators review and verify claims, with final approval or rejection handled by Academic Managers, ensuring transparency and efficiency throughout the claim process.

---

## **Features**

- **Claim Submission**:  
  Lecturers can submit claims for hours worked and upload supporting documents. Real-time status tracking is available for lecturers to monitor their claims.
  
- **Coordinator Review**:  
  Coordinators review claims and approve or reject them before passing them on to Academic Managers for final approval.
  
- **Final Approval**:  
  Academic Managers provide the final decision on submitted claims.

- **Role-Based Access**:  
  Authentication and session management are implemented for different user roles, including Lecturers, Coordinators, and Academic Managers.

---

## **User Roles**

1. **Lecturer**:  
   Submit claims, upload supporting documents, and track the status of submitted claims.

2. **Coordinator**:  
   Review and verify claims submitted by lecturers, then approve or reject them for final review.

3. **Academic Manager**:  
   Review claims after the Coordinator's approval and make the final decision (approve/reject).

---

## **System Architecture**

- **Frontend**:  
  HTML5, CSS3, JavaScript (jQuery).
  
- **Backend**:  
  ASP.NET Core MVC, C#, Entity Framework Core.
  
- **Database**:  
  SQL Server (LocalDb).

---

## **Database Structure**

1. **Claims**:  
   Stores details of lecturer claims, including hours worked, claim status, and supporting documents.
   
2. **Users**:  
   Stores user information and role-based access.
   
3. **Approvals**:  
   Tracks the review and approval process for each claim.

---

## **Installation Guide**

1. Install the .NET SDK and SQL Server (LocalDb).
2. Clone this repository.
3. Update the database connection string in `appsettings.json`.
4. Run the following commands in the terminal to apply migrations and run the application:
   ```bash
   dotnet ef database update
   dotnet run
