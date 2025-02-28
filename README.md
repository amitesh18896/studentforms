# studentforms
![image](https://github.com/user-attachments/assets/c8f67d03-0cca-44cc-a2c0-04a51cfe4d33)
![image](https://github.com/user-attachments/assets/f36c0aae-7cd4-4169-8bf8-e2ed1783c406)
![image](https://github.com/user-attachments/assets/93620d00-b7eb-4cbb-afda-18fe7bd082ec)




create database studentv
use studentv

CREATE TABLE Country (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE State (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CountryId INT FOREIGN KEY REFERENCES Country(Id)
);

CREATE TABLE City (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    StateId INT FOREIGN KEY REFERENCES State(Id)
);

CREATE TABLE StudentList (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Mobile NVARCHAR(15) NOT NULL,
    CountryId INT FOREIGN KEY REFERENCES Country(Id),
    StateId INT FOREIGN KEY REFERENCES State(Id),
    CityId INT FOREIGN KEY REFERENCES City(Id)
);


INSERT INTO Country (Name) VALUES 
('USA'),
('Canada'),
('India');


INSERT INTO State (Name, CountryId) VALUES 
('California', 1), 
('New York', 1), 
('Ontario', 2), 
('Quebec', 2), 
('Maharashtra', 3), 
('Karnataka', 3);






INSERT INTO City (Name, StateId) VALUES 
('Los Angeles', 1), 
('San Francisco', 1), 
('New York City', 2), 
('Buffalo', 2), 
('Toronto', 3), 
('Montreal', 4), 
('Mumbai', 5), 
('Bangalore', 6); 






INSERT INTO StudentList (FirstName, LastName, Gender, Mobile, CountryId, StateId, CityId) VALUES 
('John', 'Doe', 'Male', '1234567890', 1, 1, 1), 
('Jane', 'Smith', 'Female', '9876543210', 1, 2, 3), 
('Amit', 'Kumar', 'Male', '7894561230', 3, 5, 7),
('Priya', 'Sharma', 'Female', '8523697410', 3, 6, 8);
