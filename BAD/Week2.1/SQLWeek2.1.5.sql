use Movies;
-- Find the address of the MGM studio
SELECT * FROM Movies WHERE studioName = 'MGM studio';

-- Find Sylvester Stallone’s birthday 
SELECT name, birthday FROM movieStar WHERE name = 'Sylvester Stallone';

-- Find all movie stars which played in either a movie made after 1980 or a movie with Love in its title*
SELECT name, movieTitle, year FROM movieStar 
    INNER JOIN starsIn ON name=movieStarName
    INNER JOIN movies ON movieTitle=title and movieYear=year
    where year > 1980 or title like '%Love%';

--Find all executives worth at least 1.000.000DKK

SELECT name, netWorth FROM movieExec WHERE netWorth > 1000000; 

--Find all movie stars which are female or lives in Copenhagen

SELECT name, address, gender from movieStar WHERE address like '%Copenhagen%' or gender like '%f%';

-- Find all movie stars who participated in Star Wars: Episode IV - A New Hope

SELECT name, address, gender, birthday
from movieStar 
    INNER JOIN starsIn ON name=movieStarName 
    INNER JOIN movies ON movieTitle=title and movieYear=year
    where title = 'Star Wars: Episode IV - A New Hope';