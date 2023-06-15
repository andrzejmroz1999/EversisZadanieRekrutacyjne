 IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Employees')
  BEGIN
     CREATE DATABASE Employees;
  END