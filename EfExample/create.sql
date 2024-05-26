CREATE SCHEMA cw5;
GO

CREATE TABLE cw5.Students (
                              IdStudent int  NOT NULL IDENTITY (1,1),
                              FirstName nvarchar(120)  NOT NULL,
                              LastName nvarchar(120)  NOT NULL,
                              CONSTRAINT Students_pk PRIMARY KEY  (IdStudent)
);

CREATE TABLE cw5.Student_Group (
                                   IdStudent int  NOT NULL,
                                   IdGroup int  NOT NULL,
                                   RegisteredAt datetime  NOT NULL,
                                   CONSTRAINT Student_Group_pk PRIMARY KEY  (IdStudent,IdGroup)
);

CREATE TABLE cw5.Groups (
                            IdGroup int  NOT NULL IDENTITY(1,1),
                            Name nvarchar(120)  NOT NULL,
                            CONSTRAINT Groups_pk PRIMARY KEY  (IdGroup)
);

ALTER TABLE cw5.Student_Group ADD CONSTRAINT Student_Group_Student
    FOREIGN KEY (IdStudent)
        REFERENCES cw5.Students (IdStudent)
        ON DELETE CASCADE;

ALTER TABLE cw5.Student_Group ADD CONSTRAINT Student_Group_Group
    FOREIGN KEY (IdGroup)
        REFERENCES cw5.Groups (IdGroup)
        ON DELETE CASCADE;



INSERT INTO cw5.Students (FirstName, LastName) VALUES ('Jan', 'Kowalski');
INSERT INTO cw5.Students (FirstName, LastName) VALUES ('Anna', 'Nowak');
INSERT INTO cw5.Students (FirstName, LastName) VALUES ('Piotr', 'Kowalczyk');

INSERT INTO cw5.Groups (Name) VALUES ('Group1');
INSERT INTO cw5.Groups (Name) VALUES ('Group2');

INSERT INTO cw5.Student_Group (IdStudent, IdGroup, RegisteredAt) VALUES (1, 1, '2020-01-01');
INSERT INTO cw5.Student_Group (IdStudent, IdGroup, RegisteredAt) VALUES (2, 1, '2020-01-01');

SELECT* FROM cw5.Students;