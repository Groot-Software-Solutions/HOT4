-- RUN ONLY AFTER NEW PASSWORD HASH IS EXPECTED TO WORK

ALTER TABLE tblAccess DROP AccessPassword;
-- change PasswordHash and PasswordSalt to NOT NULL