USE MappingEx;

-- Opret Movies-tabellen
CREATE TABLE Movies (
    title CHAR(100),        -- Filmens titel (maks. 100 tegn)
    year INT,               -- Udgivelsesåret for filmen
    genre CHAR(20),         -- Filmens genre (maks. 20 tegn)
    length INT,             -- Filmens længde i minutter
    PRIMARY KEY (title, year) -- Kombineret primær nøgle af titel og år
);

-- Indsæt dummydata i Movies-tabellen
INSERT INTO Movies (title, year, genre, length) VALUES
('Inception', 2010, 'Sci-Fi', 148),
('Titanic', 1997, 'Romance', 195),
('The Matrix', 1999, 'Action', 136),
('The Godfather', 1972, 'Crime', 175),
('Pulp Fiction', 1994, 'Crime', 154);

-- Opret Product-tabellen (antaget ud fra opgaven)
CREATE TABLE Product (
    product_id INT IDENTITY(1,1), -- Unik ID for hvert produkt
    product_name NVARCHAR(100),  -- Produktnavn
    price DECIMAL(10, 2),        -- Pris på produktet
    PRIMARY KEY (product_id)     -- Primær nøgle
);

-- Indsæt dummydata i Product-tabellen
INSERT INTO Product (product_name, price) VALUES
('Popcorn', 5.99),
('Soda', 3.50),
('Candy', 2.99),
('Nachos', 6.99);

-- Vis indholdet af Movies-tabellen
SELECT * FROM Movies;

-- Vis indholdet af Product-tabellen
SELECT * FROM Product;
