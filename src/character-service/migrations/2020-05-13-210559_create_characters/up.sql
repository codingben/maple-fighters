CREATE TABLE characters (
    id SERIAL PRIMARY KEY,
    userid VARCHAR NOT NULL,
    charactername VARCHAR NOT NULL,
    index INTEGER NOT NULL,
    classindex INTEGER NOT NULL
)