CREATE TABLE characters (
    id SERIAL PRIMARY KEY,
    userid INTEGER NOT NULL,
    charactername VARCHAR NOT NULL,
    index INTEGER NOT NULL,
    classindex INTEGER NOT NULL,
    mapindex INTEGER NOT NULL
)