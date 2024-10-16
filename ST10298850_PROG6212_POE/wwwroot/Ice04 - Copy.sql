-- ICE 05 

CREATE TABLE EMPLOYEES_Ice04 (
    EMPLOYEEID NUMBER PRIMARY KEY,
    FIRST_NAME VARCHAR2(50),
    LAST_NAME VARCHAR2(50),
    JOB_TITLE VARCHAR2(50),
    SALARY NUMBER,
    HIRE_DATE DATE
);


INSERT INTO EMPLOYEES_Ice04 (EMPLOYEEID, FIRST_NAME, LAST_NAME, JOB_TITLE, SALARY, HIRE_DATE) 
VALUES (101, 'John', 'Doe', 'Manager', 6000, TO_DATE('2016-09-27', 'YYYY-MM-DD'));

INSERT INTO EMPLOYEES_Ice04 (EMPLOYEEID, FIRST_NAME, LAST_NAME, JOB_TITLE, SALARY, HIRE_DATE) 
VALUES (102, 'Jane', 'Smith', 'Salesman', 4000, TO_DATE('2018-09-27', 'YYYY-MM-DD'));

INSERT INTO EMPLOYEES_Ice04 (EMPLOYEEID, FIRST_NAME, LAST_NAME, JOB_TITLE, SALARY, HIRE_DATE) 
VALUES (103, 'Emily', 'Johnson', 'Clerk', 3500, TO_DATE('2017-09-27', 'YYYY-MM-DD'));

DECLARE
    -- Declare variables
    v_employee_id EMPLOYEES_Ice04.EMPLOYEEID%TYPE;
    v_first_name EMPLOYEES_Ice04.FIRST_NAME%TYPE;
    v_last_name EMPLOYEES_Ice04.LAST_NAME%TYPE;
    v_job_title EMPLOYEES_Ice04.JOB_TITLE%TYPE;
    v_salary EMPLOYEES_Ice04.SALARY%TYPE;
    v_hire_date EMPLOYEES_Ice04.HIRE_DATE%TYPE;
    v_years_of_service NUMBER;
    v_bonus NUMBER;

    -- Create a cursor to fetch employee data
    CURSOR employee_cursor IS
        SELECT EMPLOYEEID, FIRST_NAME, LAST_NAME, JOB_TITLE, SALARY, HIRE_DATE
        FROM EMPLOYEES_Ice04;

BEGIN
    -- Open the cursor
    OPEN employee_cursor;

    -- Loop through each record
    LOOP
        FETCH employee_cursor INTO v_employee_id, v_first_name, v_last_name, v_job_title, v_salary, v_hire_date;
        EXIT WHEN employee_cursor%NOTFOUND;  -- Exit the loop if no more records

        -- Calculate years of service
        v_years_of_service := FLOOR(MONTHS_BETWEEN(SYSDATE, v_hire_date) / 12);

        -- Determine the bonus based on job title and years of service
        IF v_job_title = 'Manager' THEN
            IF v_salary > 5000 THEN
                v_bonus := v_salary * 0.10;  -- 10% bonus for Managers with salary > 5000
            ELSE
                v_bonus := v_salary * 0.05;  -- 5% bonus for Managers with salary <= 5000
            END IF;
        ELSIF v_job_title = 'Salesman' THEN
            v_bonus := v_years_of_service * 100;  -- $100 for each year of service for Salesman
        ELSE
            IF v_years_of_service > 5 THEN
                v_bonus := v_salary * 0.03;  -- 3% bonus for other employees with more than 5 years of service
            ELSE
                v_bonus := 0;  -- No bonus for other employees with 5 or fewer years of service
            END IF;
        END IF;

        -- Output employee details and bonus with the desired format
        DBMS_OUTPUT.PUT_LINE('ID: ' || v_employee_id || ', Name: ' || v_first_name || ' ' || v_last_name || 
                             ', Job Title: ' || v_job_title || ', Salary: ' || v_salary || 
                             ', Years OF Service: ' || v_years_of_service || ', Bonus: ' || v_bonus);
    END LOOP;

    -- Close the cursor
    CLOSE employee_cursor;

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('An error occurred: ' || SQLERRM);
END;
/
-- END OF ICE PART 1

--START OF ICE PART 2  

CREATE TABLE "EMPLOYEES_Ice04-2" (
    EMPLOYEEID NUMBER PRIMARY KEY,
    FIRST_NAME VARCHAR2(50),
    LAST_NAME VARCHAR2(50),
    JOB_TITLE VARCHAR2(50),
    SALARY NUMBER,
    HIRE_DATE DATE,
    PERFORMANCE_FLAG VARCHAR2(20)  -- Added the performance_flag column
);

INSERT INTO "EMPLOYEES_Ice04-2" (EMPLOYEEID, FIRST_NAME, LAST_NAME, SALARY, PERFORMANCE_FLAG)
VALUES (101, 'John', 'Doe', 4000, 'Y');

INSERT INTO "EMPLOYEES_Ice04-2" (EMPLOYEEID, FIRST_NAME, LAST_NAME, SALARY, PERFORMANCE_FLAG)
VALUES (102, 'Jane', 'Smith', 3500, 'N');

INSERT INTO "EMPLOYEES_Ice04-2" (EMPLOYEEID, FIRST_NAME, LAST_NAME, SALARY, PERFORMANCE_FLAG)
VALUES (103, 'Emily', 'Johnson', 3000, 'N');

CREATE OR REPLACE PROCEDURE ProcessPayroll(
    p_employee_id EMPLOYEES_Ice04_2.EMPLOYEEID%TYPE,
    p_sales NUMBER
) IS
    v_bonus NUMBER;
    v_performance_flag CHAR(1);
BEGIN
    -- Check sales and calculate bonus using IF-THEN-ELSE
    IF p_sales > 50000 THEN
        v_bonus := 1500;
    ELSIF p_sales BETWEEN 35000 AND 50000 THEN
        v_bonus := 500;
    ELSE
        v_bonus := 100;
    END IF;

    -- Fetch performance flag from the table for the employee
    SELECT performance_flag
    INTO v_performance_flag
    FROM EMPLOYEES_Ice04_2
    WHERE EMPLOYEEID = p_employee_id;

    -- Handle special cases with CASE for performance flag
    CASE v_performance_flag
        WHEN 'Y' THEN
            v_bonus := v_bonus * 1.10; -- 10% extra bonus
    END CASE;

    -- Output the employee's bonus
    DBMS_OUTPUT.PUT_LINE('Employee ID: ' || p_employee_id || ' Bonus: ' || v_bonus);

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Employee not found for ID: ' || p_employee_id);
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('An error occurred: ' || SQLERRM);
END ProcessPayroll;