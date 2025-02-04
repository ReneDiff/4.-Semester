--1.	Map a 1-1 relationship between a customer and his/hers address

CREATE TABLE Address(
    id bigint IDENTITY(0,1), -- Opretter en automatisk stigende unik ID-kolonne for Address-tabellen.
    street NVARCHAR(50),     -- Kolonne til gadenavn med op til 50 tegn.
    city NVARCHAR(50),       -- Kolonne til bynavn med op til 50 tegn.
    CONSTRAINT pk_Address PRIMARY KEY (id), -- Definerer id som primær nøgle for tabellen.
);

CREATE TABLE Customer(
    id bigint IDENTITY(0,1), -- Opretter en automatisk stigende unik ID-kolonne for Customer-tabellen.
    firstName NVARCHAR(50),  -- Kolonne til fornavn med op til 50 tegn.
    lastName NVARCHAR(100),  -- Kolonne til efternavn med op til 100 tegn.
    addressId bigint UNIQUE, -- Unik constraint på addressId, sikrer at én kunde har én adresse.
    CONSTRAINT pk_customer PRIMARY KEY (id), -- Definerer id som primær nøgle for tabellen.
    CONSTRAINT fk_Address FOREIGN KEY (addressId) REFERENCES Address(id) -- Angiver en fremmednøgle til Address-tabellen.
        on UPDATE CASCADE -- Sikrer, at ændringer i Address-tabellen (id) opdateres i Customer-tabellen.
);