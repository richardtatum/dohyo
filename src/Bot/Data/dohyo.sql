CREATE TABLE IF NOT EXISTS user (
    id INTEGER PRIMARY KEY NOT NULL,
    balance INTEGER NOT NULL DEFAULT 1000,
    username TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS fight (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    start_date DATE NOT NULL,
    end_date DATE NULL,
    winning_side TEXT NULL
 );

CREATE TABLE IF NOT EXISTS bet (
    fight_id INTEGER NOT NULL,
    user_id INTEGER NOT NULL,
    amount INTEGER NOT NULL,
    side TEXT NOT NULL,
    winning_bet BOOLEAN NULL,
    FOREIGN KEY (fight_id) REFERENCES fight(id),
    FOREIGN KEY (user_id) REFERENCES user(id)
);

INSERT INTO user 
(id, balance, username) 
VALUES (1, 0, 'Wizard');