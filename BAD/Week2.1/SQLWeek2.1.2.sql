USE MappingEx;

--3.	Create a 1-N relationship between a customer and a number of orders.

CREATE TABLE Customer(
    id bigint IDENTITY(0,1), -- Opretter en automatisk stigende unik ID-kolonne for Customer-tabellen.
    firstName NVARCHAR(50),  -- Kolonne til fornavn med op til 50 tegn.
    lastName NVARCHAR(100),  -- Kolonne til efternavn med op til 100 tegn.
    CONSTRAINT pk_Customer PRIMARY KEY (id) -- Definerer id som primær nøgle for tabellen.
);

----named COrder as 'Order' is a keyword
----decided to make this a weak entity
CREATE TABLE COrder(
    id bigint IDENTITY(0,1), -- Opretter en automatisk stigende unik ID-kolonne for COrder-tabellen.
    customerId bigint,       -- Kolonne til at gemme reference til en kunde via deres id.
    CONSTRAINT pk_COrder PRIMARY key (id), -- Definerer id som primær nøgle for tabellen.
    CONSTRAINT fk_Customer FOREIGN key (customerId) REFERENCES Customer(id) -- Angiver en fremmednøgle til Customer-tabellen.
        on UPDATE CASCADE -- Hvis Customer-id opdateres, opdateres det også i COrder-tabellen.
        on DELETE CASCADE -- Hvis en kunde slettes, slettes også alle deres ordrer (svag entitet).
)


