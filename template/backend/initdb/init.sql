-- Create Users table if it doesn't exist
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    "Username" character varying(50) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "Role" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Insert initial admin user
INSERT INTO "Users" ("Username", "Password", "Phone", "Email", "Status", "Role", "CreatedAt")
VALUES (
    'admin',
    'Admin123', -- Note: In a real application, this should be a hashed password
    '1234567890',
    'admin@email.com',
    'Active',
    'Admin',
    CURRENT_TIMESTAMP
); 