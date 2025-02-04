--2.	Change the relationship, such that multiple customers can use the same address

CREATE TABLE Address(
    id bigint IDENTITY(0,1),
    street NVARCHAR(50),
    city NVARCHAR(50),
    CONSTRAINT pk_Address PRIMARY KEY (id)
);

CREATE TABLE Customer(
    id bigint IDENTITY(0,1),
    firstName NVARCHAR(50),
    lastName NVARCHAR(100),
    addressId bigint,
    CONSTRAINT pk_Customer PRIMARY KEY (id),
    CONSTRAINT fk_Address FOREIGN KEY (addressId) REFERENCES Address(id) 
        on UPDATE CASCADE
);