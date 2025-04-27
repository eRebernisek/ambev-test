-- Create Users table if it doesn't exist
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid DEFAULT gen_random_uuid() PRIMARY KEY,
    "Username" character varying(50) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Phone" character varying(20) NOT NULL,
    "Email" character varying(100) NOT NULL,
    "Status" character varying(20) NOT NULL,
    "Role" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp with time zone NULL
);

-- Insert initial admin user
INSERT INTO "Users" ("Username", "Password", "Phone", "Email", "Status", "Role", "CreatedAt", "UpdatedAt")
VALUES (
    'admin',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewLxNyYHm4L.Cavu', -- BCrypt hashed password for 'Admin123'
    '1234567890',
    'admin@email.com',
    'Active',
    'Admin',
    CURRENT_TIMESTAMP,
    NULL
); 