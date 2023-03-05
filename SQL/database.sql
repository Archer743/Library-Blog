CREATE DATABASE library;
USE library;
CREATE TABLE IF NOT EXISTS users(
user_id INT PRIMARY KEY AUTO_INCREMENT,
username VARCHAR(50) NOT NULL,
email_address VARCHAR(100) NOT NULL UNIQUE,
country VARCHAR(50) DEFAULT 'Unknown',
birthdate DATE NOT NULL,
gender ENUM('male', 'female', 'other'),
status ENUM('student', 'teacher', 'other')
);

CREATE TABLE IF NOT EXISTS admins(
admin_id INT,
CONSTRAINT FK_admins_users FOREIGN KEY(admin_id) REFERENCES users(user_id)
);

CREATE TABLE IF NOT EXISTS experts(
expert_id INT,
CONSTRAINT FK_experts_users FOREIGN KEY(expert_id) REFERENCES users(user_id)
);
CREATE TABLE IF NOT EXISTS fav_books (
    user_id INT NOT NULL,
    book_id INT NOT NULL,
    CONSTRAINT FK_fav_books_users FOREIGN KEY(user_id) REFERENCES users(id),
    CONSTRAINT FK_fav_books_books FOREIGN KEY(book_id) REFERENCES books(id),
    CONSTRAINT PK_fav_books PRIMARY KEY (user_id, book_id)
);
