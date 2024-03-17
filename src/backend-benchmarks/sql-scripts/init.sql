CREATE TABLE IF NOT EXISTS Fragments (
    Id BIGSERIAL PRIMARY KEY,
    Content TEXT
);

CREATE TABLE IF NOT EXISTS Branches (
    Id BIGSERIAL PRIMARY KEY,
    FromFragmentId BIGINT,
    ToFragmentId BIGINT,
    Description VARCHAR(255),
    FOREIGN KEY (FromFragmentId) REFERENCES Fragments(Id),
    FOREIGN KEY (FromFragmentId) REFERENCES Fragments(Id)
);